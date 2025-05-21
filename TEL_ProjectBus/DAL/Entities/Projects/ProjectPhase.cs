using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;


public class ProjectPhase : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	//public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}
