﻿using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

namespace TEL_ProjectBus.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProjectQueryController(IRequestClient<GetProjectByIdQuery> _getProjectClient,
	IRequestClient<GetProjectsQuery> _getProjectsClient,
	IRequestClient<GetProjectProfileQuery> _getProjectProfileByIdClient,
	ILogger<ProjectQueryController> _logger
	) : BaseApiController
{
	/// <summary>
	/// Возвращает проект по указанному идентификатору.
	/// </summary>
	[HttpGet("projects/{id:int}")]
	public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized("User ID is required.");
		}

		try
		{
			var query = new GetProjectByIdQuery
			{
				ProjectId = id,
				UserId = userId,
			};
			var response = await _getProjectClient.GetResponse<ProjectDto>(query, cancellationToken);

			return Ok(response.Message);
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while getting project {Id} (user {UserId}): {Fault}", id, userId, ex.Message);
			return MapFaultToHttp(ex);
		}
	}

	/// <summary>
	/// Получает список проектов доступных авторизованному пользователю
	/// с учетом фильтрации и пагинации.
	/// </summary>
	/// <param name="pageNumber">Номер страницы для пагинации (по умолчанию 1).</param>
	/// <param name="pageSize">Размер страницы для пагинации (по умолчанию 20).</param>
	/// <param name="projectName">Фильтр по имени проекта (по умолчанию пустое значение).</param>
	/// <param name="projectCode">Фильтр по коду проекта (по умолчанию пустое значение).</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Возвращает список проектов в соответствии с указанными параметрами фильтрации и пагинации.</returns>
	[HttpGet("projects")]
	public async Task<IActionResult> GetProjects(
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 20,
		[FromQuery] string projectName = "",
		[FromQuery] string projectCode = "",
		CancellationToken cancellationToken = default)
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized("User ID is required.");
		}

		try
		{
			var query = new GetProjectsQuery
			{
				PageNumber = pageNumber,
				PageSize = pageSize,
				ProjectNameFilter = projectName,
				ProjectCodeFilter = projectCode,

				UserId = userId,
			};
			var resp = await _getProjectsClient.GetResponse<GetProjectsResponse>(query, cancellationToken);

			return Ok(resp.Message);
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while getting all projects by user {UserId}: {Fault}", userId, ex.Message);
			return MapFaultToHttp(ex);
		}
	}

	/// <summary>
	/// Возвращает паспорт проекта по указанному идентификатору.
	/// </summary>
	/// <param name="id">Идентификатор проекта.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Возвращает данные профиля проекта по указанному ID. 
	/// В случае ошибки или таймаута — соответствующий статус (например, 504 — Request Timeout или 404 — Not Found).</returns>
	[HttpGet("projects/{id:int}/profile")]
	public async Task<IActionResult> GetProjectProfileById(int id, CancellationToken cancellationToken)
	{
		_logger.LogInformation($"[ProjectQueryController] Sending GetBudgetByIdQuery for {id}");

		try
		{
			var response = await _getProjectProfileByIdClient.GetResponse<GetProjectProfileResponse>(
				new GetProjectProfileQuery { ProjectId = id }, cancellationToken);

			return ApiOk(response.Message);
		}
		catch (RequestTimeoutException ex)
		{
			_logger.LogError(ex, "[ProjectQueryController] Timeout when requesting ProjectProfile by ID {Id}", id);
			return StatusCode(504, "Request Timeout: " + ex.Message);
		}
		catch (RequestFaultException ex) when (ex.Message.Contains("ProjectNotFound"))
		{
			_logger.LogWarning("Project {ProjectId} not found", id);
			return NotFound();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "[ProjectQueryController] Error when requesting ProjectProfile by ID {Id}", id);
			return StatusCode(500, "Internal Server Error: " + ex.Message);
		}
	}
}