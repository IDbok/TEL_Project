namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record GetBudgetsByProjectIdQuery
{
    public int ProjectId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
