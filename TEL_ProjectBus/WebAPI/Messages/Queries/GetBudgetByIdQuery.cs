namespace TEL_ProjectBus.WebAPI.Messages.Queries;

public record GetBudgetByIdQuery
{
	public Guid BudgetItemId { get; init; }
}
