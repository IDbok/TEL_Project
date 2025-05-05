using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Entities.TestUnits;

/// <summary>
/// Общая абстракция для всех сущностей, у которых нужно вести аудит изменений.
/// </summary>
public abstract class AuditableEntity
{
	public DateTime DateChanged { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }
}

#region \u2192 Справочники

public class BudgetGroup
{
	public int Id { get; set; }
	public string BudgetGroupName { get; set; } = string.Empty;
	public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}

public class Classifier
{
	public Guid Id { get; set; }
	public string ClassifierCode { get; set; } = string.Empty;
	public ICollection<Project> Projects { get; set; } = new List<Project>();
}

public class ApproveStatus
{
	public Guid Id { get; set; }
	public string StatusName { get; set; } = string.Empty;
	public ICollection<ProjectApproveStatus> ProjectApproveStatuses { get; set; } = new List<ProjectApproveStatus>();
	public ICollection<BudgetApprove> BudgetApproves { get; set; } = new List<BudgetApprove>();
}

public class ProjectStage
{
	public int Id { get; set; }
	public string StageName { get; set; } = string.Empty;
	public ICollection<ProjectApproveStatus> ProjectApproveStatuses { get; set; } = new List<ProjectApproveStatus>();
	public ICollection<ProjectApproverTemplate> Templates { get; set; } = new List<ProjectApproverTemplate>();
}

public class RefTaskStatus
{
	public int Id { get; set; }
	public string TaskStatusName { get; set; } = string.Empty;
	public ICollection<TaskStatus> TaskStatuses { get; set; } = new List<TaskStatus>();
}

#endregion

#region \u2192 Project approval / параметры

public class ProjectApproveStatus : AuditableEntity
{
	public long Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid ApproveStatusId { get; set; }
	public Guid? ApprovedBy { get; set; }
	public int ProjectStageId { get; set; }

	public Project Project { get; set; } = null!;
	public ApproveStatus ApproveStatus { get; set; } = null!;
	public ProjectStage ProjectStage { get; set; } = null!;
	public User? Approver { get; set; }
}

public class ProjectApproverTemplate
{
	public Guid Id { get; set; }
	public int ProjectStageId { get; set; }
	public Guid? RoleId { get; set; } // для IdentityRole.Id
	public Guid? UserId { get; set; }

	public ProjectStage ProjectStage { get; set; } = null!;
	public User? User { get; set; }
}

/// <summary>
/// Расширенные параметры проекта, вынесенные в отдельную таблицу.
/// </summary>
public class ProjectParameter : AuditableEntity
{
	public Guid Id { get; set; }
	public Guid ProjectId { get; set; }
	public Guid? ProjectOwner { get; set; }
	public DateTime? ProjectBegin { get; set; }
	public DateTime? ProjectEnd { get; set; }
	public Guid? ClassifierId { get; set; }
	public int? PhaseId { get; set; }
	public int? ProjectStageId { get; set; }
	public int? ProjectStatusId { get; set; }
	public string? ProjectDescription { get; set; }

	public Project Project { get; set; } = null!;
	public Classifier? Classifier { get; set; }
}

#endregion

#region \u2192 Budget approve / версии

public class BudgetApprove : AuditableEntity
{
	public Guid Id { get; set; }
	public Guid BudgetId { get; set; }
	public int? RoleId { get; set; } // Если роли вынесены в отдельную справоч. табл.
	public Guid? UserId { get; set; }
	public Guid ApproveStatusId { get; set; }
	public bool Approved { get; set; }
	public string? Comment { get; set; }

	public Budget Budget { get; set; } = null!;
	public ApproveStatus ApproveStatus { get; set; } = null!;
	public User? User { get; set; }
}

public class BudgetVersionParameter
{
	public Guid Id { get; set; }
	public Guid BudgetId { get; set; }
	public string VersionName { get; set; } = string.Empty;

	public Budget Budget { get; set; } = null!;
}

#endregion

#region \u2192 Tasks

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
	public ICollection<TaskProgress> Progresses { get; set; } = new List<TaskProgress>();
	public ICollection<TaskStatus> Statuses { get; set; } = new List<TaskStatus>();
	public ICollection<TaskParameter> Parameters { get; set; } = new List<TaskParameter>();
	public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();
}

public class TaskStatus
{
	// Композитный ключ из TaskId + StatusId
	public Guid TaskId { get; set; }
	public int StatusId { get; set; }
	public DateTime DateChanged { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }

	public Task Task { get; set; } = null!;
	public RefTaskStatus Status { get; set; } = null!;
}

public class TaskProgress
{
	// Композитный ключ из TaskId + DateChanged (или Guid Id)
	public Guid TaskId { get; set; }
	public int ProgressPercentage { get; set; }
	public DateTime DateChanged { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }

	public Task Task { get; set; } = null!;
}

public class TaskParameter
{
	// Композитный ключ из TaskId + Key
	public Guid TaskId { get; set; }
	public string Key { get; set; } = string.Empty;
	public string? Value { get; set; }

	public Task Task { get; set; } = null!;
}

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

#endregion
