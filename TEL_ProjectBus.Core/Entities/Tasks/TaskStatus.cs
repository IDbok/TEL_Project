namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskStatus // Ref_TaskStatus
{
	// Композитный ключ из TaskId + StatusId
	public Guid TaskId { get; set; }
	public int StatusId { get; set; }
	public DateTime DateChanged { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }

	public Task Task { get; set; } = null!;
	public RefTaskStatus Status { get; set; } = null!;
}
