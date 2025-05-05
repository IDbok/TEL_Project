namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectStage
{
	public int Id { get; set; }
	public string StageName { get; set; } = string.Empty;
	public ICollection<ProjectApproveStatus> ProjectApproveStatuses { get; set; } = [];
	public ICollection<ProjectApproverTemplate> Templates { get; set; } = [];
}
