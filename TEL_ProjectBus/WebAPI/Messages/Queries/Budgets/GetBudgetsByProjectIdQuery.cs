namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record GetBudgetsByProjectIdQuery
{
	public int ProjectId { get; init; }
}
