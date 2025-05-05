using TEL_ProjectBus.DAL.Configurations;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectPhase
{
	public int Id { get; set; }
	public string PhaseName { get; set; } = string.Empty;
	public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}
