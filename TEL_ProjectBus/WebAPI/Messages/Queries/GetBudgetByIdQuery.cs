namespace TEL_ProjectBus.WebAPI.Messages.Queries;

public record GetBudgetByIdQuery
{
	public long BudgetItemId { get; init; }
}
