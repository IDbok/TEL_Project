namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class Expense
{
	public Guid Id { get; set; }
	public Guid BudgetId { get; set; }
	public decimal Amount { get; set; }
	public string Description { get; set; } = string.Empty;
	public decimal RgpPercent { get; set; }
	public DateTime DateChanged { get; set; }
	public Guid ChangedBy { get; set; }

	public Budget Budget { get; set; } = null!;
}

