using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

namespace TEL_ProjectBus.WebAPI.Controllers.Budget;

[AllowAnonymous]
public class BudgetQueryController(IRequestClient<GetBudgetsQuery> _getBudgetsClient,
	IRequestClient<GetBudgetByIdQuery> _getBudgetByIdClient,
	IRequestClient<GetBudgetsByProjectIdQuery> _getBudgetsByProjectIdClient,
	ILogger<BudgetQueryController> _logger
	)
	: BaseApiController
{
	/// <summary>
	/// Возвращает список строк бюджета проекта по указанному идентификатору.
	/// </summary>
	[HttpGet("projects/{projectId:int}/budgets")]
	public async Task<IActionResult> GetByProjectId(
		int projectId, 
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 20,
		CancellationToken cancellationToken = default)
	{
		try
		{
			var response = await _getBudgetsByProjectIdClient.GetResponse<GetBudgetsByProjectIdResponse>(
				new GetBudgetsByProjectIdQuery
				{
					ProjectId = projectId,
					PageNumber = pageNumber,
					PageSize = pageSize,
				},
				cancellationToken
			);

			return Ok(response.Message);
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while getting budgets to Project {Id}: {Fault}", projectId, ex.Message);
			return MapFaultToHttp(ex);
		}
	}

	/// <summary>
	/// Возвращает бюджет по указанному идентификатору.
	/// </summary>
	[HttpGet("budgets/{id:int}")]
	public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
	{
		try
		{
			var response = await _getBudgetByIdClient.GetResponse<BudgetLineDto>(
				new GetBudgetByIdQuery { BudgetItemId = id }, cancellationToken);

			return Ok(response.Message);               
		}
		catch (RequestFaultException ex)
		{
			_logger.LogWarning("Fault while getting budget {Id}: {Fault}", id, ex.Message);
			return MapFaultToHttp(ex);
		}
	}
}
