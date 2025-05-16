using TEL_ProjectBus.BLL.DTOs;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;




public record GetBudgetByIdResponse
{
	public Guid BudgetItemId { get; init; }
	public string ArticleNumber1C { get; init; } = null!;
	public bool VisibleOnPipeline { get; init; }
	public string BudgetName { get; init; } = null!;
	public string Role { get; init; } = null!;
	public decimal? Hours { get; init; }
	public string Nomenclature { get; init; } = null!;
	public string Classifier { get; init; } = null!;
	public int Quantity { get; init; }
	public string EC { get; init; } = null!;
	public decimal CP_TCC_Pcs { get; init; }
	public decimal RGP_PcsPercent { get; init; }
	public decimal Probability { get; init; }
	public DateTime PlannedDate { get; init; }
	public DateTime CreatedAt { get; init; }
	public List<BudgetLineDto> budgetLines { get; init; } = new();
}
