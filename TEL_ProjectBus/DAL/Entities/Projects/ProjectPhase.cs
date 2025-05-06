using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Projects;


public class ProjectPhase : AuditableEntity
{
	public int Id { get; set; }
	public string PhaseName { get; set; } = string.Empty;
	public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}
