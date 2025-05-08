using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

// Ref_BudgetGroup
public class BudgetGroup : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public ICollection<Budget> Budgets { get; } = new HashSet<Budget>();
}
