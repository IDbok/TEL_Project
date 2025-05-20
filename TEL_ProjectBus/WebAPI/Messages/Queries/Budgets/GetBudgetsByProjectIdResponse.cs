using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record GetBudgetsByProjectIdResponse : ResponseBase
{
	public List<BudgetLineDto> Budgets { get; init; } = new();
	public int TotalCount => Budgets.Count();
}
