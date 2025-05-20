using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record GetBudgetByIdResponse : ResponseBase
{
	public BudgetLineDto? BudgetLine { get; init; }
}
