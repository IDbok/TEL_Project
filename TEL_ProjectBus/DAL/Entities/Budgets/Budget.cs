namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class Budget
{
	public Guid Id { get; set; }
	public int BudgetGroupId { get; set; }
	public bool VisOnPipeline { get; set; }
	public Guid BudgetErpId { get; set; }
	public decimal ManHoursCost { get; set; }
	public string BudgetName { get; set; } = string.Empty;
	public Guid ProjectId { get; set; }
	public int BudgetVersion { get; set; }
	public DateTime DateChanged { get; set; }
	public Guid ChangedBy { get; set; }

	//public ICollection<BudgetItem> BudgetItems { get; set; }
	public ICollection<Expense> Expenses { get; set; } = [];
}

