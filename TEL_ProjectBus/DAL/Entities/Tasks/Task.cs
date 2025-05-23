﻿using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

public class Task : AuditableEntity, IHasIdentity<long>
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public string OwnerId { get; set; } = null!;
	public string AuthorId { get; set; } = null!;
	public DateTime Start { get; set; }
	public DateTime End { get; set; }

	public User Owner { get; set; } = null!;
	public User Author { get; set; } = null!;

	public ICollection<TaskProgress> Progresses { get; set; } = [];
	public ICollection<TaskStatus> Statuses { get; set; } = [];
	public ICollection<TaskParameter> Parameters { get; set; } = [];
	public ICollection<TaskAttachment> Attachments { get; set; } = [];
}
