using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.Messages.Commands;

namespace TEL_ProjectBus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetBusController : ControllerBase
{
	private readonly IPublishEndpoint _publishEndpoint;

	public BudgetBusController(IPublishEndpoint publishEndpoint)
	{
		_publishEndpoint = publishEndpoint;
	}

	/// <summary>
	/// Создаёт новую бюджетную запись (BudgetItem), используя переданную команду CreateBudgetItemCommand, 
	/// и публикует соответствующее сообщение через IPublishEndpoint.
	/// Возвращает статус «202 Accepted» в случае успешной публикации.
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create-budget-item")]
	public async Task<IActionResult> CreateBudgetItem(CreateBudgetItemCommand command)
	{
		await _publishEndpoint.Publish(command);
		return Accepted();
	}

	/// <summary>
	/// Обновляет существующую бюджетную запись на основе данных команды UpdateBudgetItemCommand 
	/// и отправляет обновлённую информацию через IPublishEndpoint.
	/// Возвращает статус «202 Accepted», если операция завершена успешно.
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPut("update-budget-item")]
	public async Task<IActionResult> UpdateBudgetItem(UpdateBudgetItemCommand command)
	{
		await _publishEndpoint.Publish(command);
		return Accepted();
	}

	/// <summary>
	/// Удаляет бюджетную запись, идентифицированную по id (типа Guid).
	/// Для удаления генерируется команда DeleteBudgetItemCommand, которая публикуется через IPublishEndpoint.
	/// При успешной операции возвращается статус «202 Accepted».
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("delete-budget-item/{id:guid}")]
	public async Task<IActionResult> DeleteBudgetItem(Guid id)
	{
		await _publishEndpoint.Publish(new DeleteBudgetItemCommand { BudgetItemId = id });
		return Accepted();
	}

	/// <summary>
	/// Создаёт новую бюджетную строку (BudgetLine) на основе команды ...
	/// Возвращает статус «202 Accepted»
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("create-budget-line")]
	public async Task<IActionResult> CreateBudgetLine(CreateBudgetItemCommand command)
	{
		//await _publishEndpoint.Publish(command);
		return Accepted();
	}

	/// <summary>
	/// Обновляет существующую бюджетную строку на основании данных команды ... 
	/// При успешном выполнении возвращает статус «202 Accepted».
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPut("update-budget-line")]
	public async Task<IActionResult> UpdateBudgetLine(UpdateBudgetItemCommand command)
	{
		//await _publishEndpoint.Publish(command);
		return Accepted();
	}

	/// <summary>
	/// Удаляет бюджетную строку, получая её идентификатор id типа Guid.
	/// Возвращает статус «202 Accepted».
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("delete-budget-line/{id:guid}")]
	public async Task<IActionResult> DeleteBudgetLine(Guid id)
	{
		//await _publishEndpoint.Publish(new DeleteBudgetItemCommand { BudgetItemId = id });
		return Accepted();
	}
}
