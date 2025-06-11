using MassTransit;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class UpdateBudgetConsumer(BudgetService budgetService, ILogger<UpdateBudgetConsumer> logger) : IConsumer<UpdateBudgetCommand>
{
	public async Task Consume(ConsumeContext<UpdateBudgetCommand> context)
	{
		var command = context.Message;

		if (await budgetService.UpdateBudgetAutoAsync(command, context.CancellationToken))
		{
			await context.RespondAsync(new UpdateBudgetResponse
			{
				IsSuccess = true,
				Message = "Budget updated successfully",
			});
		}
		else
		{
			await context.RespondAsync(new UpdateBudgetResponse
			{
				IsSuccess = false,
				Message = "Failed to update budget",
			});
		}
	}
}
