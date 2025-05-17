using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Controllers.Common;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

namespace TEL_ProjectBus.WebAPI.Controllers.Budget;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
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
	[HttpGet("projects/{id:int}/budgets")]
	public async Task<IActionResult> GetBudgetsByProjectId(int projectId)
	{
		var response = await _getBudgetByIdClient.GetResponse<GetBudgetsByProjectIdResponse>(
			new GetBudgetsByProjectIdQuery { ProjectId = projectId }, timeout: TimeSpan.FromSeconds(30));

		return ApiOk(response.Message);
	}

	/// <summary>
	/// Возвращает бюджет по указанному идентификатору.
	/// </summary>
	[ApiExplorerSettings(IgnoreApi = true)] 
	[HttpGet("budgets/{id:int}")]
	public async Task<IActionResult> GetBudgetById(int id)
	{
		var response = await _getBudgetByIdClient.GetResponse<GetBudgetByIdResponse>(
			new GetBudgetByIdQuery { BudgetItemId = id }, timeout: TimeSpan.FromSeconds(30));

		return ApiOk(response.Message);
	}

}
