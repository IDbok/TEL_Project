using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Messages.Commands;
using TEL_ProjectBus.WebAPI.Messages.Queries;

namespace TEL_ProjectBus.WebAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class ProjectBusController : BaseApiController
{
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly IRequestClient<UpdateProjectProfileCommand> _updateProjectProfileClient;
	private readonly ILogger<ProjectQueryController> _logger;


	public ProjectBusController(IPublishEndpoint publishEndpoint, ILogger<ProjectQueryController> logger)
	{
		_publishEndpoint = publishEndpoint;
		_logger = logger;
	}

	/// <summary>
	/// Обновляет паспорт проекта на основе данных команды UpdateBudgetItemCommand 
	/// и отправляет обновлённую информацию через IPublishEndpoint.
	/// Возвращает статус «202 Accepted», если операция завершена успешно.
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPut("update-budget-item")]
	public async Task<IActionResult> UpdateProjectProfileItem(UpdateProjectProfileCommand command)
	{
		var response = await _updateProjectProfileClient.GetResponse<UpdateProjectProfileResponse>(command);
		//await _publishEndpoint.Publish(command);
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
