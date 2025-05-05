namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskProgress
{
	// Композитный ключ из TaskId + DateChanged (или Guid Id)
	public Guid TaskId { get; set; }
	public int ProgressPercentage { get; set; }
	public DateTime DateChanged { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }

	public Task Task { get; set; } = null!;
}
