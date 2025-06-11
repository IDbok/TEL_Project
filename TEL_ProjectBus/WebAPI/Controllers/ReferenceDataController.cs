using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.BLL;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.WebAPI.Controllers.Common;

namespace TEL_ProjectBus.WebAPI.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class ReferenceDataController(RefDataService refDataService) : BaseApiController
{
	/// <summary>
	/// Получает список доступных ролей.
	/// </summary>
	/// <returns>Список ролей.</returns>
	[HttpGet("reference-data/roles")]
	public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
	{
		var roles = await refDataService.GetAllRolesAsync(cancellationToken);
		return ApiOk(roles);
	}

	/// <summary>
	/// Получает список всех групп бюджета.
	/// </summary>
	/// <returns>Список групп бюджета.</returns>
	[HttpGet("reference-data/budget-groups")]
	public async Task<IActionResult> GetBudgetGroups(CancellationToken cancellationToken)
	{
		var roles = await refDataService.GetAllBudgetGroupsAsync(cancellationToken);
		return ApiOk(roles);
	}
}
