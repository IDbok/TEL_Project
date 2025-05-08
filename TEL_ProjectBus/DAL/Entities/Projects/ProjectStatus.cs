using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectStatus : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string StatusName { get; set; } = string.Empty;
	public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}

