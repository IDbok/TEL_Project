using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectParameter : AuditableEntity, IHasIdentity<long>
{
	public long Id { get; set; }
	public int ProjectId { get; set; }
	public string ProjectOwnerId { get; set; } = null!;
	public int ClassifierId { get; set; }
	public int ProjectPhaseId { get; set; }
	public int ProjectStageId { get; set; }
	public int ProjectStatusId { get; set; }
	public DateTime ProjectBegin { get; set; }
	public DateTime ProjectEnd { get; set; }
	public string? Description { get; set; }

	public Project Project { get; set; } = null!;
	public User ProjectOwner { get; set; } = null!;
	public Classifier Classifier { get; set; } = null!;
	public ProjectPhase ProjectPhase { get; set; } = null!;
	public ProjectStage ProjectStage { get; set; } = null!;
	public ProjectStatus ProjectStatus { get; set; } = null!;
}
