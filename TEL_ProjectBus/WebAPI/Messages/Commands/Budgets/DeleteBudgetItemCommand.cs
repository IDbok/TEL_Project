namespace TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

public record DeleteBudgetItemCommand
{
	public Guid BudgetItemId { get; init; }
}
