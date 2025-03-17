namespace TEL_ProjectBus.Messages.Events;

public record BudgetItemDeletedEvent
{
	public Guid BudgetItemId { get; init; }
	public DateTime DeletedAt { get; init; }

}
