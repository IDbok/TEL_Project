namespace TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

public record UpdateBudgetItemCommand
{
	public Guid BudgetItemId { get; init; }
	public bool? VisibleOnPipeline { get; init; }
	public string BudgetName { get; init; }
	public string Role { get; init; }
	public decimal? Hours { get; init; }
	public string Nomenclature { get; init; }
	public string Classifier { get; init; }
	public int? Quantity { get; init; }
	public string EC { get; init; }
	public decimal? CP_TCC_Pcs { get; init; }
	public decimal? RGP_PcsPercent { get; init; }
	public decimal? Probability { get; init; }
	public DateTime? PlannedDate { get; init; }
}
