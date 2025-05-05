namespace TEL_ProjectBus.DAL.Entities.Common;

/// <summary>
/// Общая абстракция для всех сущностей, у которых нужно вести аудит изменений.
/// </summary>
public abstract class AuditableEntity
{
	public DateTime DateChanged { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }
}
