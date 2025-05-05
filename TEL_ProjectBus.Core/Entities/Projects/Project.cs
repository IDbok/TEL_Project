using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public enum ProjectStatus
{
	NotStarted,
	InProgress,
	Completed
}

public class Project
{
	public Guid Id { get; set; }
	public string ProjectName { get; set; } = string.Empty;
	public string ProjectCode { get; set; } = string.Empty;
	public DateTime DateInitiation { get; set; }
	public Guid CustomerId { get; set; }
	public Guid ClassifierId { get; set; }

	public ICollection<Budget> Budgets { get; set; } = [];

	public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
	public int ProjectProgress { get; set; } = 0;
}
