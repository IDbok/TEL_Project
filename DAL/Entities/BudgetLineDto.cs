namespace DAL.Entities;
public class BudgetLineDto
{
	public Guid Id { get; set; }

	public Guid BudgetItemId { get; set; } // внешний ключ

	public string WBSTemplate { get; set; } = string.Empty;
	public string BudgetName { get; set; } = string.Empty;
	public string Role { get; set; } = string.Empty;
	public int Hours { get; set; }
	public decimal Quantity { get; set; }
	public decimal CPTCCPcs { get; set; }
	public decimal RGPPCS { get; set; }
	public int Probability { get; set; }
	public DateTime DataPlan { get; set; }
	public decimal CPTCCPlan { get; set; }
	public DateTime? DataFact { get; set; }
	public decimal PriceTCCPcs { get; set; }
	public decimal PriceTCC { get; set; }

	// Навигационное свойство
	public BudgetItemDto? BudgetItem { get; set; }
}
