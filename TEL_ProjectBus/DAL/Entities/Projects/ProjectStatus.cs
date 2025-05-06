using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectStatus : AuditableEntity
{
	public int Id { get; set; }
	public string StatusName { get; set; } = string.Empty;
	public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}

