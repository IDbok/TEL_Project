using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class ProjectBusController : BaseApiController
{
	private readonly IRequestClient<UpdateProjectProfileCommand> _updateProjectProfileClient;
	private readonly IRequestClient<CreateProjectCommand> _createProjectClient;
	private readonly ILogger<ProjectBusController> _logger;


	public ProjectBusController(IRequestClient<UpdateProjectProfileCommand> requestClient,
		IRequestClient<CreateProjectCommand> createProjectClient,
		ILogger<ProjectBusController> logger )
	{
		_updateProjectProfileClient = requestClient;
		_createProjectClient = createProjectClient;
		_logger = logger;
	}

	/// <summary>
	/// Обновляет паспорт проекта на основе данных команды UpdateProjectProfileCommand.
	/// </summary>
	/// <param name="command"></param>
	/// <returns>Возвращает статус 202 (Accepted), если создание прошло успешно. 
	/// В противном случае — статус 400 (BadRequest) с сообщением об ошибке.</returns>
	[HttpPut("projects/update-profile")]
	public async Task<IActionResult> UpdateProjectProfile(UpdateProjectProfileCommand command)
	{
		var response = await _updateProjectProfileClient.GetResponse<UpdateProjectProfileResponse>(command);
		
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
	/// Создаёт новый проекта на основе данных команды CreateProjectCommand.
	/// В случае успешного добавления возвращает ID нового проекта.
	/// </summary>
	/// <param name="command"></param>
	/// <returns>Возвращает статус 202 (Accepted) с данными о созданном проекте, включая его ID, если создание прошло успешно. 
	/// В противном случае — статус 400 (BadRequest) с сообщением об ошибке.</returns>
	[HttpPost("projects/create")]
	public async Task<IActionResult> CreateProject(CreateProjectCommand command)
	{
		var response = await _createProjectClient.GetResponse<CreateProjectResponse>(command);

		if (response.Message.IsSuccess)
		{
			return Accepted(response.Message);
		}
		else
		{
			return BadRequest(response.Message);
		}
	}
}
