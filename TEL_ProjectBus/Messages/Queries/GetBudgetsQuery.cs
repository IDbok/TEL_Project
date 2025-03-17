namespace TEL_ProjectBus.Messages.Queries;

public record GetBudgetsQuery
{
	public int PageNumber { get; init; } = 1;
	public int PageSize { get; init; } = 20;
	public string BudgetNameFilter { get; init; } = "";
	public string ArticleNumber1CFilter { get; init; } = "";
	public string RoleFilter { get; init; } = "";
}
