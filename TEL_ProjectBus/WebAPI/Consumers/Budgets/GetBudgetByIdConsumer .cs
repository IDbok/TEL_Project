using MassTransit;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class GetBudgetByIdConsumer(BudgetService budgetService, ILogger<GetBudgetByIdConsumer> logger) : IConsumer<GetBudgetByIdQuery>
{
	public async Task Consume(ConsumeContext<GetBudgetByIdQuery> context)
	{
		try
		{
			logger.LogInformation($"[GetBudgetByIdConsumer] Received query for {context.Message.BudgetItemId}");

			var budgetItemId = context.Message.BudgetItemId;

			var dto = await budgetService.GetBudgetByIdAsync<BudgetLineDto>(budgetItemId, context.CancellationToken);

			if (dto == null)
			{
				logger.LogWarning($"[GetBudgetByIdConsumer] Budget with ID {budgetItemId} not found.");
				await context.RespondAsync(new GetBudgetByIdResponse
				{
					IsSuccess = false,
					Message = $"Budget with ID {budgetItemId} not found."
				}, context.CancellationToken);
				return;
			}

			await context.RespondAsync(new GetBudgetByIdResponse() {
				IsSuccess = true,
				Message = "Budget found.",
				BudgetLine = dto } , context.CancellationToken);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, $"[GetBudgetByIdConsumer] Error while processing GetBudgetByIdQuery: {ex.Message}");
			await context.RespondAsync(new GetBudgetByIdResponse
			{
				IsSuccess = false,
				Message = ex.Message
			}, context.CancellationToken);
		}
	}
}
