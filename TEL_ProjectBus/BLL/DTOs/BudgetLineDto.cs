namespace TEL_ProjectBus.BLL.DTOs;

public record BudgetLineDto
{
	public Guid BudgetLineId { get; init; }
	public string ExpenseItem1C { get; set; }
	public int? VisOnPipeline { get; set; }
	public string WBSTemplate { get; set; }
	public string BudgetName { get; set; }
	public string Role { get; set; }
	public int? Hours { get; set; }
	public string Nomenclature { get; set; }
	public string Classifier { get; set; }
	public int? Quantity { get; set; }
	public string EC { get; set; }
	public decimal? CPTCCPcs { get; set; }
	public int? RGPPCS { get; set; }
	public int Probability { get; set; }
	public DateTime DataPlan { get; set; }
	public decimal? CPTCCPlan { get; set; }
	public DateTime DataFact { get; set; }
	public decimal? CPTCCFact { get; set; }
	public decimal? PriceTCCPcs { get; set; }
	public decimal? PriceTCC { get; set; }
	public decimal? CV { get; set; }
	public decimal? SV { get; set; }
	public decimal? EV { get; set; }
	public decimal? CPI { get; set; }
	public decimal? SPI { get; set; }
}
