namespace TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

public record DeleteBudgetCommand
{
	public long BudgetId { get; init; }
}
