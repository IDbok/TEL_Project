using System.ComponentModel.DataAnnotations.Schema;

namespace TEL_ProjectBus.DAL.Entities.Common;

/// <summary>
/// Общая абстракция для всех сущностей, у которых нужно вести аудит изменений.
/// </summary>
public abstract class AuditableEntity
{
	public DateTime? DateChanged { get; set; }
	public string? ChangedByUserId { get; set; } = null!;
	[NotMapped] public User? ChangedByUser { get; set; } = null!;
}
