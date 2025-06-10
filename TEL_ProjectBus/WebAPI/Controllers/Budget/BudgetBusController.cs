using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Common;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Controllers.Budget;

[AllowAnonymous]
[Route("api/[controller]")]
public class BudgetBusController(IRequestClient<CreateBudgetCommand> _createClient, 
	IRequestClient<UpdateBudgetCommand> _updateClient,
	IRequestClient<DeleteBudgetCommand> _deleteClient,
	ILogger<BudgetBusController> _logger
	) : BaseApiController
{
	/// <summary>
	/// Создаёт новую бюджетную запись (BudgetItem), используя переданную команду CreateBudgetCommand.
	/// Возвращает статус «201 CreatedAtAction» в случае успешной публикации.
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("budgets/create")]
	public async Task<IActionResult> Create([FromBody] CreateBudgetCommand command)
	{
		//var resp = await _createClient.GetResponse<BudgetCreatedDto>(command);
		////var resp = await _createClient.GetResponse<CreateBudgetResponse>(command);

		//return CreatedAtAction(nameof(Create), 
		//	new { budgetId = resp.Message.BudgetId },
		//	new ApiResponse<BudgetCreatedDto>(new BudgetCreatedDto( resp.Message.BudgetId ) ));
		////resp.Message);
		////return SendResponse(resp);

		//var (success, fault) =
		//	await _createClient.GetResponse<BudgetCreatedDto, Fault<CreateBudgetCommand>>(command);

		//if (success.IsCompletedSuccessfully)
		//{
		//	var budgetId = success.Result.Message.BudgetId;
		//	_logger.LogInformation($"Budget created successfully with ID: {budgetId}");
		//	return CreatedAtAction(nameof(Create), 
		//		new { budgetId }, 
		//		new ApiResponse<BudgetCreatedDto>(success.Result.Message));
		//}
		//else
		//{
		//	var errors = string.Join("; ",
		//			 fault.Result.Message.Exceptions.Select(e => e.Message));

		//	var errorMessage = fault.Result.Message.Exceptions.FirstOrDefault()?.Message ?? "Unknown error";
		//	_logger.LogError($"Error creating budget: {errorMessage}");
		//	return BadRequest(new ApiResponse<string>($"Error creating budget: {errorMessage}"));
		//}

		try
		{
			var response = await _createClient.GetResponse<BudgetCreatedDto>(command);

			var id = response.Message.BudgetId;          // успех
			_logger.LogInformation("Budget {Id} created", id);

			return CreatedAtAction(nameof(Create),
				new { budgetId = id },
				new ApiResponse<BudgetCreatedDto>(response.Message));
		}
		catch (RequestFaultException ex)
		{
			var details = string.Join("; ",
				ex.Fault?.Exceptions.Select(e => e.Message) ?? ["Unknown error"]);
			
			// Optionally, log the full exception object for diagnostics
			_logger.LogWarning("CreateBudget fault: {Details}\nFull Exception: {Exception}", details, ex);

			// Return detailed error information to the client (consider security implications)
			return BadRequest(new ApiResponse<string>(details));
		}

	}

	/// <summary>
	/// Обновляет существующую бюджетную запись на основе данных команды UpdateBudgetCommand.
	/// Возвращает статус «202 Accepted», если операция завершена успешно.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPut("budgets/{id:long}/update")]
	public async Task<IActionResult> Update(long id, [FromBody] UpdateBudgetCommand command)
	{
		command.SetBudgetId(id);

		var resp = await _updateClient.GetResponse<UpdateBudgetResponse>(command);

		return SendResponse(resp);
	}

	/// <summary>
	/// Удаляет бюджетную запись, идентифицированную по id (типа long).
	/// При успешной операции возвращается статус «204 NoContent».
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("budgets/{id:long}/delete")]
	public async Task<IActionResult> Delete(long id)
	{
		var resp = await _deleteClient.GetResponse<DeleteBudgetResponse>(new DeleteBudgetCommand { BudgetId = id });

		return NoContent();
		//return SendResponse(resp);
	}

}
