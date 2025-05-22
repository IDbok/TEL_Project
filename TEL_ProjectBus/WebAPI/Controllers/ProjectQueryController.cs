using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

namespace TEL_ProjectBus.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProjectQueryController(IRequestClient<GetProjectsQuery> _getProjectsClient,
	IRequestClient<GetProjectProfileQuery> _getProjectProfileByIdClient,
	ILogger<ProjectQueryController> _logger
	) : BaseApiController
{
	/// <summary>
	/// Получает список проектов доступных авторизованному пользователю
	/// с учетом фильтрации и пагинации.
	/// </summary>
	/// <param name="pageNumber">Номер страницы для пагинации (по умолчанию 1).</param>
	/// <param name="pageSize">Размер страницы для пагинации (по умолчанию 20).</param>
	/// <param name="projectName">Фильтр по имени проекта (по умолчанию пустое значение).</param>
	/// <param name="projectCode">Фильтр по коду проекта (по умолчанию пустое значение).</param>
	/// <returns>Возвращает список проектов в соответствии с указанными параметрами фильтрации и пагинации.</returns>
	[HttpGet("projects")]
	public async Task<IActionResult> GetProjects(
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 20,
		[FromQuery] string projectName = "",
		[FromQuery] string projectCode = "")
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();

		if(string.IsNullOrEmpty(userId))
		{
			return Unauthorized("User ID is required.");
		}

		var query = new GetProjectsQuery
		{
			PageNumber = pageNumber,
			PageSize = pageSize,
			ProjectNameFilter = projectName,
			ProjectCodeFilter = projectCode,

			UserId = userId,
		};

		var response = await _getProjectsClient.GetResponse<GetProjectsResponse>(query);

		return ApiOk(response.Message);
	}

	/// <summary>
	/// Возвращает паспорт проекта по указанному идентификатору.
	/// </summary>
	/// <param name="id">Идентификатор проекта.</param>
	/// <returns>Возвращает данные профиля проекта по указанному ID. 
	/// В случае ошибки или таймаута — соответствующий статус (например, 504 — Request Timeout или 404 — Not Found).</returns>
	[HttpGet("projects/{id:int}/profile")]
	public async Task<IActionResult> GetProjectProfileById(int id)
	{
		_logger.LogInformation($"[ProjectQueryController] Sending GetBudgetByIdQuery for {id}");

		try
		{
			var response = await _getProjectProfileByIdClient.GetResponse<GetProjectProfileResponse>(
				new GetProjectProfileQuery { ProjectId = id }, timeout: TimeSpan.FromSeconds(30));

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