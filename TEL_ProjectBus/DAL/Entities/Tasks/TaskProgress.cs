using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskProgress : AuditableEntity, IHasIdentity<long>
{
	// Может ли Задание иметь несколько прогрессов? => да, текущий прогресс определяется по дате изменения

	public long Id { get; set; } 
	public long TaskId { get; set; } 
	public int ProgressPercentage { get; set; }

	public Task Task { get; set; } = null!;
}
