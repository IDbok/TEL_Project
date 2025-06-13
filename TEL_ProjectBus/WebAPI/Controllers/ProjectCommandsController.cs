using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class ProjectCommandsController(
        //IRequestClient<UpdateProjectProfileCommand> _updateProjectProfileClient,
        IRequestClient<CreateProjectCommand> _createClient,
        IRequestClient<UpdateProjectCommand> _updateClient,
		//IRequestClient<DeleteProjectCommand> _deleteClient,
		IPublishEndpoint _publisher,
		ILogger<ProjectCommandsController> _logger)
	: BaseApiController
{
	/// <summary>
	/// Создаёт новый проекта на основе данных команды CreateProjectCommand.
	/// В случае успешного добавления возвращает ID нового проекта.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>
	/// Возвращает статус 201 (Created) с данными о созданном проекте (включая его ID),
	/// если создание прошло успешно. 
	/// </returns>
	[HttpPost("projects")]
    public async Task<IActionResult> CreateProject(CreateProjectCommand command,
		CancellationToken cancellationToken)
    {
		try
		{
			var resp = await _createClient.GetResponse<CreateProjectResponse>(command,
			cancellationToken);

			var dto = resp.Message;
			_logger.LogInformation("Project {Id} created", dto.ProjectId);

			return CreatedAtAction(
				actionName: nameof(ProjectQueryController.GetById), // метод контроллера-чтения
				controllerName: "ProjectQuery", // имя контроллера
				new { id = dto.ProjectId }, // параметр запроса GetById
				dto);		
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("CreateProject fault: {Exception}", ex);
			return MapFaultToHttp(ex);
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
	/// </returns>
	[HttpPut("projects/{id:int}")]
    public async Task<IActionResult> Update(int id, 
		[FromBody] UpdateProjectCommand command,
		CancellationToken cancellationToken)
    {
		try
		{
			var userId = User.Identity?.Name;
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized("User ID is required.");
			}

			command = command with { Id = id, ChangedByUserId = userId, DateChanged = DateTime.UtcNow };

			var resp = await _updateClient.GetResponse<UpdateProjectResponse>(command,
				cancellationToken);
			_logger.LogInformation("Project {Id} updated", id);

			return AcceptedAtAction(
				actionName: nameof(ProjectQueryController.GetById), // метод контроллера-чтения
				controllerName: "ProjectQuery", // имя контроллера
				new { id }, // параметр запроса GetById
				resp.Message);
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while updating project {Id}: {Fault}", id, ex);
			return MapFaultToHttp(ex);
		}
	}

	/// <summary>
	/// Удаляет проект по идентификатору на основе команды DeleteProjectCommand.
	/// </summary>
	/// <param name="id">Идентификатор проекта, который требуется удалить.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>
	/// Возвращает статус 202 (Accepted), если удаление команда успешн обработана.
	/// </returns>
	[HttpDelete("projects/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {        
		try
		{//var response = await _deleteClient.GetResponse<DeleteProjectResponse>(
			await _publisher.Publish(
				new DeleteProjectCommand { ProjectId = id },
				cancellationToken);

			return Accepted();
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while deleting project {Id}: {Fault}", id, ex);
			return MapFaultToHttp(ex);
		}
	}



	///// <summary>
	///// Обновляет паспорт проекта на основе данных команды UpdateProjectProfileCommand.
	///// </summary>
	///// <param name="id"></param>
	///// <param name="command"></param>
	///// <param name="cancellationToken"></param>
	///// <returns>
	///// Возвращает статус 202 (Accepted), если создание прошло успешно.
	///// </returns>
	//[HttpPut("projects/{id:int}/profile")]
	//public async Task<IActionResult> UpdateProjectProfile(int id, UpdateProjectProfileCommand command,
	//	CancellationToken cancellationToken)
	//{
	//	var userId = User.Identity?.Name;
	//	if (string.IsNullOrEmpty(userId))
	//	{
	//		return Unauthorized("User ID is required.");
	//	}

	//	command = command with { ProjectId = id, ChangedByUserId = userId, DateChanged = DateTime.UtcNow };

	//	var response = await _updateProjectProfileClient.GetResponse<UpdateProjectProfileResponse>(command,
	//		cancellationToken);

	//	if (response.Message.IsSuccess)
	//	{
	//		return Accepted(response.Message);
	//	}
	//	else
	//	{
	//		return BadRequest(response.Message);
	//	}
	//}
}
