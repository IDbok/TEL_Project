namespace TEL_ProjectBus.DAL.Entities;

public enum ProjectStatus
{
	NotStarted,
	InProgress,
	Completed
}

public class Project
{
	public Guid Id { get; set; }
	public string ProjectName { get; set; }
	public string ProjectCode { get; set; }
	public DateTime DateInitiation { get; set; }
	public Guid CustomerId { get; set; }
	public Guid ClassifierId { get; set; }

	public ICollection<Budget> Budgets { get; set; }

	public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
	public int ProjectProgress { get; set; } = 0;
}
