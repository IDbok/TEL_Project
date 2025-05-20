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
	/// Возвращает статус «202 Accepted» в случае успешной публикации.
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	[HttpPost("budgets/create")]
	public async Task<IActionResult> Create([FromBody] CreateBudgetCommand command)
	{
		var resp = await _createClient.GetResponse<CreateBudgetResponse>(command);
		return SendResponse(resp);
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
	/// При успешной операции возвращается статус «202 Accepted».
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpDelete("budgets/{id:long}/delete")]
	public async Task<IActionResult> Delete(long id)
	{
		var resp = await _deleteClient.GetResponse<DeleteBudgetResponse>(new DeleteBudgetCommand { BudgetId = id });

		return SendResponse(resp);
	}


	private IActionResult SendResponse<T>(Response<T> resp)
		where T : BasResponseBase
	{
		if (resp.Message.IsSuccess)
		{
			return ApiOk(resp.Message);
		}
		else
		{
			return BadRequest(resp.Message);
		}
	}
}
