using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Controllers.Budget;

[AllowAnonymous]
public class BudgetCommandsController(IRequestClient<CreateBudgetCommand> _createClient, 
	IRequestClient<UpdateBudgetCommand> _updateClient,
	//IRequestClient<DeleteBudgetCommand> _deleteClient,
	IPublishEndpoint _publisher,
	ILogger<BudgetCommandsController> _logger
	) : BaseApiController
{
	/// <summary>
	/// Создаёт новую бюджетную запись (BudgetItem), используя переданную команду CreateBudgetCommand.
	/// Возвращает статус «201 CreatedAtAction» в случае успешной публикации.
	/// </summary>
	/// <param name="command"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost("budgets")]
	public async Task<IActionResult> Create([FromBody] CreateBudgetCommand command, 
		CancellationToken cancellationToken)
	{
		try
		{
			var resp = await _createClient.GetResponse<BudgetCreatedDto>(command, cancellationToken);

			var dto = resp.Message;
			_logger.LogInformation("Budget {Id} created", dto.BudgetId);

			return CreatedAtAction(
				actionName: nameof(BudgetQueryController.GetById), // метод контроллера-чтения
				controllerName: "BudgetQuery", // имя контроллера
				new { id = dto.BudgetId }, // параметр запроса GetById
				dto);
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("CreateBudget fault: {Exception}", ex);
			return MapFaultToHttp(ex);
		}
	}

	/// <summary>
	/// Обновляет существующую бюджетную запись на основе данных команды UpdateBudgetCommand.
	/// Возвращает статус «202 Accepted», если операция завершена успешно.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="command"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPut("budgets/{id:long}")]
	public async Task<IActionResult> Update(long id, [FromBody] UpdateBudgetCommand command,
		CancellationToken cancellationToken)
	{
		try
		{
			command = command with { Id = id};

			//var resp = await _updateClient.GetResponse<UpdateBudgetResponse>(command, cancellationToken);
			await _publisher.Publish(command, cancellationToken);
			_logger.LogInformation("Budget {Id} published", id);

			return AcceptedAtAction(
				actionName: "GetById",
				controllerName: "BudgetQuery",
				new { id },
				$"Budget {id} published.");
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while updating budget {Id}: {Fault}", id, ex);
			return MapFaultToHttp(ex);
		}
	}

	/// <summary>
	/// Удаляет бюджетную запись, идентифицированную по id (типа long).
	/// При успешной отправке операции в очередь возвращается статус «202 Accepted».
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>
	/// Возвращает статус 202 (Accepted), если удаление команда успешн обработана.
	/// </returns>
	[HttpDelete("budgets/{id:long}")]
	public async Task<IActionResult> Delete(long id,
		CancellationToken cancellationToken)
	{
		try
		{//var resp = await _deleteClient.GetResponse<DeleteBudgetResponse>(
			await _publisher.Publish(
				new DeleteBudgetCommand { BudgetId = id }, cancellationToken);

			return Accepted();
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while deleting budget {Id}: {Fault}", id, ex);
			return MapFaultToHttp(ex);
		}
	}

}
