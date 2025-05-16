using MassTransit;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class GetBudgetsConsumer : IConsumer<GetBudgetsQuery>
{
	public async Task Consume(ConsumeContext<GetBudgetsQuery> context)
	{
		var query = context.Message;

		// TODO: Здесь обращение к базе данных с фильтрацией и пагинацией.
		// Псевдокод:
		//var budgetsFromDb = Database.BudgetItems
		//	.Where(x =>
		//		(string.IsNullOrEmpty(query.BudgetNameFilter) || x.BudgetName.Contains(query.BudgetNameFilter)) &&
		//		(string.IsNullOrEmpty(query.ArticleNumber1CFilter) || x.ArticleNumber1C.Contains(query.ArticleNumber1CFilter)) &&
		//		(string.IsNullOrEmpty(query.RoleFilter) || x.Role == query.RoleFilter))
		//	.Skip((query.PageNumber - 1) * query.PageSize)
		//	.Take(query.PageSize)
		//	.ToList();

		//var totalCount = Database.BudgetItems.Count(); // общее количество записей с фильтрами

		//// Формируем ответ
		//await context.RespondAsync(new GetBudgetsResponse
		//{
		//	Items = budgetsFromDb.Select(x => new BudgetItemDto
		//	{
		//		BudgetItemId = x.Id,
		//		ArticleNumber1C = x.ArticleNumber1C,
		//		VisibleOnPipeline = x.VisibleOnPipeline,
		//		BudgetName = x.BudgetName,
		//		Role = x.Role,
		//		Hours = x.Hours,
		//		CP_TCC_Pcs = x.CP_TCC_Pcs,
		//		PlannedDate = x.PlannedDate
		//	}),
		//	TotalCount = totalCount,
		//	PageNumber = query.PageNumber,
		//	PageSize = query.PageSize
		//});
	}
}
