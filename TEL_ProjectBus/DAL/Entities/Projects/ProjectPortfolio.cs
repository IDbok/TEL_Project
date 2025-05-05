using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectPortfolio : AuditableEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public ICollection<ProjectPortfolioCross> Projects { get; set; } = [];
}
