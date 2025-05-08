using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Reference;

public class ApproveStatus : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string StatusName { get; set; } = string.Empty;
	public ICollection<ProjectApproveStatus> ProjectApproveStatuses { get; set; } = [];
}
