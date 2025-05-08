using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class ProjectVO : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
}
