using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class Task : AuditableEntity
{
	public Guid Id { get; set; }
	public Guid ProjectId { get; set; }
	public string TaskName { get; set; } = string.Empty;
	public string TaskDescription { get; set; } = string.Empty;
	public Guid TaskOwnerId { get; set; }
	public Guid TaskAuthorId { get; set; }
	public DateTime TaskStart { get; set; }
	public DateTime? TaskEnd { get; set; }

	public Project Project { get; set; } = null!;
	public User TaskOwner { get; set; } = null!;
	public User TaskAuthor { get; set; } = null!;
	public ICollection<TaskProgress> Progresses { get; set; } = [];
	public ICollection<TaskStatus> Statuses { get; set; } = [];
	public ICollection<TaskParameter> Parameters { get; set; } = [];
	public ICollection<TaskAttachment> Attachments { get; set; } = [];
}
