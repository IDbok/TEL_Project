using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class TaskAttachment : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public long TaskId { get; set; }
	public byte[] Content { get; set; } = Array.Empty<byte>();
	public string Description { get; set; } = string.Empty;
	public string SourceOfFile { get; set; } = string.Empty;
	public string FileName { get; set; } = string.Empty;
	public string FileFormat { get; set; } = string.Empty;
	public int FileVersion { get; set; }

	public Task Task { get; set; } = null!;
}
