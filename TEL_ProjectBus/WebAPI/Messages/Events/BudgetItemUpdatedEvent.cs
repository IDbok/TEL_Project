namespace TEL_ProjectBus.WebAPI.Messages.Events;

public record BudgetItemUpdatedEvent
{
	public Guid BudgetItemId { get; init; }
	public DateTime UpdatedAt { get; init; }
}
