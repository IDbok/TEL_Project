using System.ComponentModel;

namespace TEL_ProjectBus.DAL.Enums;

public enum ProjectStatusEnum
{
	[Description("В работе")]
	AtWork = 1,
	[Description("На удержании")]
	HoldOn = 2,
	[Description("Завершён")]
	Ended = 3,
	[Description("Отклонён")]
	Cancelled = 4,
}
