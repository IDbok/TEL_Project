using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskStatus : AuditableEntity
{
	public long Id { get; set; }
	public long TaskId { get; set; }
	public int StatusId { get; set; }

	public Task Task { get; set; } = null!;
	public RefTaskStatus Status { get; set; } = null!;
}
