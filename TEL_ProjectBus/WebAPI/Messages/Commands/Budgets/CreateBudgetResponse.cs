using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

public record CreateBudgetResponse : BasResponseBase
{
	public long BudgetId { get; init; }
}
