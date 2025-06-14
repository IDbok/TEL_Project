using MassTransit;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class UpdateBudgetConsumer(BudgetService budgetService, ILogger<UpdateBudgetConsumer> logger) : IConsumer<UpdateBudgetCommand>
{
	public async Task Consume(ConsumeContext<UpdateBudgetCommand> context)
	{
		var command = context.Message;
		await budgetService.UpdateBudgetAutoAsync(command, context.CancellationToken);
	}
}
