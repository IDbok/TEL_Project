﻿using MassTransit;
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
	[HttpGet("projects/{projectId:int}/budgets")]
	public async Task<IActionResult> GetByProjectId(int projectId)
	{
		var response = await _getBudgetsByProjectIdClient.GetResponse<GetBudgetsByProjectIdResponse>(
			new GetBudgetsByProjectIdQuery { ProjectId = projectId }, timeout: TimeSpan.FromSeconds(30));

		return SendResponse(response);
	}

	/// <summary>
	/// Возвращает бюджет по указанному идентификатору.
	/// </summary>
	[HttpGet("budgets/{id:int}")]
	public async Task<IActionResult> GetById(int id)
	{
		var response = await _getBudgetByIdClient.GetResponse<GetBudgetByIdResponse>(
			new GetBudgetByIdQuery { BudgetItemId = id }, timeout: TimeSpan.FromSeconds(30));

		return SendResponse(response);
	}

}
