namespace DAL.Entities;
public class BudgetItemDto
{
	public Guid Id { get; set; }

	public string ArticleNumber1C { get; set; } = string.Empty;
	public bool VisibleOnPipeline { get; set; }
	public string BudgetName { get; set; } = string.Empty;
	public string Role { get; set; } = string.Empty;
	public int Hours { get; set; }
	public string Nomenclature { get; set; } = string.Empty;
	public string Classifier { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public string EC { get; set; } = string.Empty;
	public decimal CP_TCC_Pcs { get; set; }
	public decimal RGP_PcsPercent { get; set; }
	public int Probability { get; set; }
	public DateTime PlannedDate { get; set; }
	public DateTime CreatedAt { get; set; }

	// Навигационное свойство - коллекция строк
	public ICollection<BudgetLineDto> BudgetLines { get; set; } = new List<BudgetLineDto>();
}
