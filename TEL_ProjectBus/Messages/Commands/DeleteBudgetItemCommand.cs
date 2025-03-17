namespace TEL_ProjectBus.Messages.Commands;

public record DeleteBudgetItemCommand
{
	public Guid BudgetItemId { get; init; }
}
