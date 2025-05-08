using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskStatus : AuditableEntity, IHasIdentity<long>
{
	public long Id { get; set; }
	public long TaskId { get; set; }
	public int StatusId { get; set; }

	public Task Task { get; set; } = null!;
	public RefTaskStatus Status { get; set; } = null!;
}
