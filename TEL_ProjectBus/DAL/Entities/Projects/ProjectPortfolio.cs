using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectPortfolio : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }

	public ICollection<ProjectPortfolioCross> ProjectLinks { get; } = new HashSet<ProjectPortfolioCross>();
}
