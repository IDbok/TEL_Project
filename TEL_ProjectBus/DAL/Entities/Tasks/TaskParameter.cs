using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Entities.Tasks;

// todo: вопрос по архитектуре. если связь с проектом, то получается можно одно задание в разные проекты поставить?

// cвязь с проектом через TaskParametr => да, но необязательна нужна.
// Связь задания с проектом обязательно через user
public class TaskParameter : AuditableEntity
{
	public long Id { get; set; }
	public long TaskId { get; set; }
	public int? ProjectId { get; set; }

	public int? HumanHoursResource { get; set; }

	public Task Task { get; set; } = null!;
	public Project? Project { get; set; }
}
