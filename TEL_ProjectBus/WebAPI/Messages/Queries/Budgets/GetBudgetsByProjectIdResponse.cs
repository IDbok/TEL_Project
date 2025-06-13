using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

public record GetBudgetsByProjectIdResponse
{
    public IEnumerable<BudgetLineDto> Items { get; init; } = new List<BudgetLineDto>();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
