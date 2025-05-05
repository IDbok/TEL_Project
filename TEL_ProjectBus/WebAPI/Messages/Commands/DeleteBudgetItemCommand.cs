namespace TEL_ProjectBus.WebAPI.Messages.Commands;

public record DeleteBudgetItemCommand
{
	public Guid BudgetItemId { get; init; }
}
