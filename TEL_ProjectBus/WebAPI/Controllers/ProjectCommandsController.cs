using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class ProjectCommandsController(
        IRequestClient<UpdateProjectProfileCommand> _updateProjectProfileClient,
        IRequestClient<CreateProjectCommand> _createProjectClient,
        IRequestClient<UpdateProjectCommand> _updateProjectClient,
        IRequestClient<DeleteProjectCommand> _deleteProjectClient,
        ILogger<ProjectCommandsController> _logger)
	: BaseApiController
{
	/// <summary>
	/// Создаёт новый проекта на основе данных команды CreateProjectCommand.
	/// В случае успешного добавления возвращает ID нового проекта.
	/// </summary>
	/// <param name="command"></param>
	/// <returns>Возвращает статус 202 (Accepted) с данными о созданном проекте, включая его ID, если создание прошло успешно. 
	/// В противном случае — статус 400 (BadRequest) с сообщением об ошибке.</returns>
    [HttpPost("projects")]
    public async Task<IActionResult> CreateProject(CreateProjectCommand command,
		CancellationToken cancellationToken)
    {
        var response = await _createProjectClient.GetResponse<CreateProjectResponse>(command,
			cancellationToken);

		if (response.Message.IsSuccess)
		{
			return Accepted(response.Message);
		}
		else
		{
			return BadRequest(response.Message);
        }
    }

	/// <summary>
	/// Обновляет проект по идентификатору на основе данных команды UpdateProjectCommand.
	/// Строки бюджета не учитываются при обновлении.
	/// </summary>
	/// <param name="id">Идентификатор проекта, который требуется обновить.</param>
	/// <param name="command">Данные для обновления проекта.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>
	/// Возвращает статус 202 (Accepted) с обновлёнными данными проекта, если обновление прошло успешно.
	/// В противном случае — статус 400 (BadRequest) с сообщением об ошибке.
	/// </returns>
	[HttpPut("projects/{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectCommand command,
		CancellationToken cancellationToken)
    {
		var userId = User.Identity?.Name;
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized("User ID is required.");
		}
		command = command with { Id = id, ChangedByUserId = userId, DateChanged = DateTime.UtcNow };
        var response = await _updateProjectClient.GetResponse<UpdateProjectResponse>(command,
			cancellationToken);
        return SendResponse(response);
    }

	/// <summary>
	/// Обновляет паспорт проекта на основе данных команды UpdateProjectProfileCommand.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="command"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>Возвращает статус 202 (Accepted), если создание прошло успешно. 
	/// В противном случае — статус 400 (BadRequest) с сообщением об ошибке.</returns>
	[HttpPut("projects/{id:int}/profile")]
	public async Task<IActionResult> UpdateProjectProfile(int id, UpdateProjectProfileCommand command,
		CancellationToken cancellationToken)
	{
		var userId = User.Identity?.Name;
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized("User ID is required.");
		}

		command = command with { ProjectId = id, ChangedByUserId = userId, DateChanged = DateTime.UtcNow };

		var response = await _updateProjectProfileClient.GetResponse<UpdateProjectProfileResponse>(command,
			cancellationToken);

		if (response.Message.IsSuccess)
		{
			return Accepted(response.Message);
		}
		else
		{
			return BadRequest(response.Message);
		}
	}


	/// <summary>
	/// Удаляет проект по идентификатору на основе команды DeleteProjectCommand.
	/// </summary>
	/// <param name="id">Идентификатор проекта, который требуется удалить.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>
	/// Возвращает статус 202 (Accepted), если удаление прошло успешно.
	/// В противном случае — статус 400 (BadRequest) с сообщением об ошибке.
	/// </returns>
	[HttpDelete("projects/{id:int}")]
    public async Task<IActionResult> DeleteProject(int id, CancellationToken cancellationToken)
    {
        var response = await _deleteProjectClient.GetResponse<DeleteProjectResponse>(
			new DeleteProjectCommand { ProjectId = id },
			cancellationToken);
        return SendResponse(response);
    }
}
