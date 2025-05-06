using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class ProjectVO : AuditableEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
}
