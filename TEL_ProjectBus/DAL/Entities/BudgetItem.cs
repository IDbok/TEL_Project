namespace TEL_ProjectBus.DAL.Entities;

public class BudgetItem
{
	public Guid Id { get; set; }
	public Guid BudgetId { get; set; }
	public decimal Amount { get; set; }
	public DateTime DatePlanned { get; set; }
	public bool BudgetActual { get; set; }

	public Budget Budget { get; set; }
}

