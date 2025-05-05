namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class RefTaskStatus
{
	public int Id { get; set; }
	public string TaskStatusName { get; set; } = string.Empty;
	public ICollection<TaskStatus> TaskStatuses { get; set; } = [];
}
