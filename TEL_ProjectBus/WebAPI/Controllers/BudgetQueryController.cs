using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Messages.Queries;

namespace TEL_ProjectBus.WebAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class BudgetQueryController : BaseApiController
{
	private readonly IRequestClient<GetBudgetsQuery> _getBudgetsClient;
	private readonly IRequestClient<GetBudgetByIdQuery> _getBudgetByIdClient;
	private readonly ILogger<BudgetQueryController> _logger;

	public BudgetQueryController(
		IRequestClient<GetBudgetsQuery> getBudgetsClient,
		IRequestClient<GetBudgetByIdQuery> getBudgetByIdClient,
		ILogger<BudgetQueryController> logger)
	{
		_getBudgetsClient = getBudgetsClient;
		_getBudgetByIdClient = getBudgetByIdClient;
		_logger = logger;
	}


	/// <summary>
	/// Возвращает список бюджетов в соответствии с фильтрами на страничку (кол-во строк на странице и её номер страницы указываются в параметрах запроса).
	/// </summary>
	[HttpGet("budgets")]
	public async Task<IActionResult> GetBudgets(
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 20,
		[FromQuery] string budgetName = "",
		[FromQuery] string articleNumber1C = "",
		[FromQuery] string role = "")
	{
		var query = new GetBudgetsQuery
		{
			PageNumber = pageNumber,
			PageSize = pageSize,
			BudgetNameFilter = budgetName,
			ArticleNumber1CFilter = articleNumber1C,
			RoleFilter = role
		};

		var response = await _getBudgetsClient.GetResponse<GetBudgetsResponse>(query);

		return ApiOk(response.Message);
	}

	/// <summary>
	/// Возвращает бюджет по указанному идентификатору.
	/// </summary>
	[HttpGet("budgets/{id:long}")]
	public async Task<IActionResult> GetBudgetById(long id)
	{
		_logger.LogInformation($"[BudgetQueryController] Sending GetBudgetByIdQuery for {id}");

		try
		{
			var response = await _getBudgetByIdClient.GetResponse<GetBudgetByIdResponse>(
				new GetBudgetByIdQuery { BudgetItemId = id }, timeout: TimeSpan.FromSeconds(30));

			return ApiOk(response.Message);
		}
		catch (RequestTimeoutException ex)
		{
			_logger.LogError(ex, "[BudgetQueryController] Timeout when requesting Budget by ID {Id}", id);
			return StatusCode(504, "Request Timeout: " + ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "[BudgetQueryController] Error when requesting Budget by ID {Id}", id);
			return StatusCode(500, "Internal Server Error: " + ex.Message);
		}
	}

}
