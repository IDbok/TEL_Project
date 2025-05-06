using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectStage : AuditableEntity
{
	public int Id { get; set; }
	public string StageName { get; set; } = string.Empty;
	public ICollection<ProjectApproveStatus> ProjectApproveStatuses { get; set; } = [];
	public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}
