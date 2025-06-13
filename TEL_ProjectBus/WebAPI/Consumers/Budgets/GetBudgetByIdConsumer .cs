using MassTransit;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.BLL.Exceptions;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class GetBudgetByIdConsumer(BudgetService budgetService, ILogger<GetBudgetByIdConsumer> logger) : IConsumer<GetBudgetByIdQuery>
{
	public async Task Consume(ConsumeContext<GetBudgetByIdQuery> context)
	{
		logger.LogInformation($"[GetBudgetByIdConsumer] Received query for {context.Message.BudgetItemId}");

		var budgetItemId = context.Message.BudgetItemId;

		var dto = await budgetService.GetBudgetByIdAsync<BudgetLineDto>(budgetItemId, context.CancellationToken);

		if (dto is null)
		{
			throw new NotFoundException($"Budget with ID {budgetItemId} not found");
		}

		await context.RespondAsync(dto);
	}
}
