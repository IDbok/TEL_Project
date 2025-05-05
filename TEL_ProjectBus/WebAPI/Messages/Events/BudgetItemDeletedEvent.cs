namespace TEL_ProjectBus.WebAPI.Messages.Events;

public record BudgetItemDeletedEvent
{
	public Guid BudgetItemId { get; init; }
	public DateTime DeletedAt { get; init; }

}
