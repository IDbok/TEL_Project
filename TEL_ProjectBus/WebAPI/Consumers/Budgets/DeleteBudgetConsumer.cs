using MassTransit;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class DeleteBudgetConsumer(BudgetService budgetService, ILogger<DeleteBudgetConsumer> logger) : IConsumer<DeleteBudgetCommand>
{
	public async Task Consume(ConsumeContext<DeleteBudgetCommand> context)
	{
		var command = context.Message;

		if (await budgetService.DeleteBudgetAsync(command.BudgetId, context.CancellationToken))
		{
			await context.RespondAsync(new DeleteBudgetResponse
			{
				IsSuccess = true,
				Message = "Budget deleted successfully",
			});
		}
		else
		{
			await context.RespondAsync(new DeleteBudgetResponse
			{
				IsSuccess = false,
				Message = "Failed to delete budget",
			});
		}
	}
}
