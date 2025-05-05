namespace TEL_ProjectBus.DAL.Entities.Common;

/// <summary>
/// Общая абстракция для всех сущностей, у которых нужно вести аудит изменений.
/// </summary>
public abstract class AuditableEntity
{
	public DateTime? DateChanged { get; set; }
	public Guid ChangedByUserId { get; set; }
	public User ChangedByUser { get; set; } = null!;
}
