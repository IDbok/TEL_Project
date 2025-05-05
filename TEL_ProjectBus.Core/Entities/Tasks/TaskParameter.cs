namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskParameter
{
	// Композитный ключ из TaskId + Key
	public Guid TaskId { get; set; }
	public string Key { get; set; } = string.Empty;
	public string? Value { get; set; }

	public Task Task { get; set; } = null!;
}
