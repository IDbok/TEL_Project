using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Entities.Projects;

/// <summary>
/// Расширенные параметры проекта, вынесенные в отдельную таблицу.
/// </summary>

public class ProjectParameter : AuditableEntity
{
	public Guid Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid? ProjectOwner { get; set; }
	public DateTime? ProjectBegin { get; set; }
	public DateTime? ProjectEnd { get; set; }
	public Guid? ClassifierId { get; set; }
	public int? PhaseId { get; set; }
	public int? ProjectStageId { get; set; }
	public int? ProjectStatusId { get; set; }
	public string? ProjectDescription { get; set; }

	public Project Project { get; set; } = null!;
	public Classifier? Classifier { get; set; }
}
