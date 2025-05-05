namespace TEL_ProjectBus.DAL.Entities.Budgets;

// Ref_BudgetGroup
public class BudgetGroup
{
	public int Id { get; set; }
	public string BudgetGroupName { get; set; } = string.Empty;
	public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
