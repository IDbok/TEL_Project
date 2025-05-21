using System.ComponentModel.DataAnnotations.Schema;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Enums;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectParameter : AuditableEntity, IHasIdentity<long>
{
	public long Id { get; set; }
	public int ProjectId { get; set; }
	public string ProjectOwnerId { get; set; } = null!;
	public long ClassifierId { get; set; }
	public DateTime ProjectBegin { get; set; }
	public DateTime ProjectEnd { get; set; }
	public string? Description { get; set; }

	public Project Project { get; set; } = null!;
	public User ProjectOwner { get; set; } = null!;
	public Classifier Classifier { get; set; } = null!;
	public ProjectPhaseEnum ProjectPhase { get; set; } 
	public ProjectStageEnum ProjectStage { get; set; }
	public ProjectStatusEnum ProjectStatus { get; set; }
}
