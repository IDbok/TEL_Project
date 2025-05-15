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
	private readonly ILogger<ProjectBusController> _logger;


	public ProjectBusController(IRequestClient<UpdateProjectProfileCommand> requestClient,
		ILogger<ProjectBusController> logger )
	{
		_updateProjectProfileClient = requestClient;
		_logger = logger;
	}

	/// <summary>
	/// Обновляет паспорт проекта на основе данных команды UpdateBudgetItemCommand 
	/// и отправляет обновлённую информацию через IPublishEndpoint.
	/// Возвращает статус «202 Accepted», если операция завершена успешно.
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPut("projects/update-profile")]
	public async Task<IActionResult> UpdateProjectProfileItem(UpdateProjectProfileCommand command)
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

	
}
