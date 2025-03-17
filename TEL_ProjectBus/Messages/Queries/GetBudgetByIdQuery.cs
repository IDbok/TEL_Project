namespace TEL_ProjectBus.Messages.Queries;

public record GetBudgetByIdQuery
{
	public Guid BudgetItemId { get; init; }
}
