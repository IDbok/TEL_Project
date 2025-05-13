using System.ComponentModel;

namespace TEL_ProjectBus.DAL.Enums;

public enum ProjectStageEnum
{
	[Description("Продажи")]
	PreSale = 1,
	[Description("Выполнение")]
	Execution = 2,
	[Description("Закрытие")]
	Closing = 3,
}
