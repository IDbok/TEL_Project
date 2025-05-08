using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class RefTaskStatus : AuditableEntity, IHasIdentity<int> // todo: переходник на перечисление RefTaskStatusEnum
{
	public int Id { get; set; }
	public string TaskStatusName { get; set; } = string.Empty;
	public ICollection<TaskStatus> TaskStatuses { get; set; } = [];
}
