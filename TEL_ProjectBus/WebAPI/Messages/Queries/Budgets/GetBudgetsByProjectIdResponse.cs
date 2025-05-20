using TEL_ProjectBus.BLL.DTOs;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record GetBudgetsByProjectIdResponse
{
	public List<BudgetLineDto> Budgets { get; init; } = new();
	public int TotalCount => Budgets.Count();
}
