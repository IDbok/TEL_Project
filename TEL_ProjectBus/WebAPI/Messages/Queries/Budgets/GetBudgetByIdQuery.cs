namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record GetBudgetByIdQuery
{
	public long BudgetItemId { get; init; }
}
