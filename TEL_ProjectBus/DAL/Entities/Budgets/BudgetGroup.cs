using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

// Ref_BudgetGroup
public class BudgetGroup : AuditableEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public ICollection<Budget> Budgets { get; } = new HashSet<Budget>();
}
