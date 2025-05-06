using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectApproveStatus : AuditableEntity
{
	public long Id { get; set; }
	public int ProjectId { get; set; }
	public int ApproveStatusId { get; set; }
	public string? ApprovedByUserId { get; set; }
	public int ProjectStageId { get; set; }

	public Project Project { get; set; } = null!;
	public ApproveStatus ApproveStatus { get; set; } = null!;
	public ProjectStage ProjectStage { get; set; } = null!;
	public User? Approver { get; set; }
}
