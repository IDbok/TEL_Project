namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskAttachment
{
	public Guid Id { get; set; }
	public Guid TaskId { get; set; }
	public byte[] Content { get; set; } = Array.Empty<byte>();
	public string Description { get; set; } = string.Empty;
	public string SourceOfFile { get; set; } = string.Empty;
	public string FileName { get; set; } = string.Empty;
	public string FileFormat { get; set; } = string.Empty;
	public int FileVersion { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }

	public Task Task { get; set; } = null!;
}
