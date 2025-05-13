using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectStage : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public ICollection<ProjectApproveStatus> ProjectApproveStatuses { get; set; } = [];
	public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}
