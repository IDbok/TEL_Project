using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Common;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Controllers.Budget;

[AllowAnonymous]
public class BudgetCommandsController(IRequestClient<CreateBudgetCommand> _createClient, 
	IRequestClient<UpdateBudgetCommand> _updateClient,
	IRequestClient<DeleteBudgetCommand> _deleteClient,
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
	public async Task<IActionResult> Create(CreateBudgetCommand command, 
		CancellationToken cancellationToken)
	{
		try
		{
			var response = await _createClient.GetResponse<BudgetCreatedDto>(command, cancellationToken);

			var id = response.Message.BudgetId;
			_logger.LogInformation("Budget {Id} created", id);

			return CreatedAtAction(
				actionName: "GetById",
				controllerName: "BudgetQuery",
				new { id },
				new ApiResponse<BudgetCreatedDto>(response.Message));
		}
		catch (RequestFaultException ex)
		{
			var details = string.Join("; ",
				ex.Fault?.Exceptions.Select(e => e.Message) ?? ["Unknown error"]);
			
			_logger.LogWarning("CreateBudget fault: {Details}\nFull Exception: {Exception}", details, ex);

			return BadRequest(new ApiResponse<string>(details));
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
	public async Task<IActionResult> Update(long id, UpdateBudgetCommand command,
		CancellationToken cancellationToken)
	{
		command.SetBudgetId(id);

		var resp = await _updateClient.GetResponse<UpdateBudgetResponse>(command, cancellationToken);

		return SendResponse(resp);
	}

	/// <summary>
	/// Удаляет бюджетную запись, идентифицированную по id (типа long).
	/// При успешной операции возвращается статус «204 NoContent».
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpDelete("budgets/{id:long}")]
	public async Task<IActionResult> Delete(long id,
		CancellationToken cancellationToken)
	{
		var resp = await _deleteClient.GetResponse<DeleteBudgetResponse>(
			new DeleteBudgetCommand { BudgetId = id }, cancellationToken);

		return NoContent();
		//return SendResponse(resp);
	}

}
