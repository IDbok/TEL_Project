using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class ProjectVOBudgetCross : IHasIdentity<int>
{
	public int Id { get; set; }
	public long BudgetId { get; set; }
	public int ProjectVoId { get; set; }

	public Budget Budget { get; set; } = null!;
	public ProjectVO ProjectVO { get; set; } = null!;
}
