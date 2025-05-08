using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class BudgetVersionParameter : IHasIdentity<long>
{
	public long Id { get; set; }
	public long BudgetId { get; set; }
	public string VersionName { get; set; } = string.Empty;

	public Budget Budget { get; set; } = null!;
}
