namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record BudgetItemDto
{
	public Guid BudgetItemId { get; init; }
	public string ArticleNumber1C { get; init; }
	public bool VisibleOnPipeline { get; init; }
	public string BudgetName { get; init; }
	public string Role { get; init; }
	public decimal? Hours { get; init; }
	public decimal CP_TCC_Pcs { get; init; }
	public DateTime PlannedDate { get; init; }
}

public record GetBudgetsResponse
{
	public IEnumerable<BudgetItemDto> Items { get; init; }
	public int TotalCount { get; init; }
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
}
