using MassTransit;
using TEL_ProjectBus.BLL.Budgets;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.WebAPI.Messages.Commands.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class CreateBudgetConsumer(BudgetService budgetService, ILogger<CreateBudgetConsumer> _logger) 
	: IConsumer<CreateBudgetCommand>
{
	public async Task Consume(ConsumeContext<CreateBudgetCommand> context)
	{
		//CreateBudgetResponse response;
		//try
		//{
		//	var newBudgetId = await budgetService.CreateNewBudgetAsync(context.Message);

		//	response = new CreateBudgetResponse
		//	{
		//		IsSuccess = true,
		//		Message = "Budget created successfully",
		//		BudgetId = newBudgetId,
		//	};
		//}
		//catch (Exception ex)
		//{
		//	_logger.LogError(ex, "Error creating budget");
		//	response = new CreateBudgetResponse
		//	{
		//		IsSuccess = false,
		//		Message = $"Error creating budget.\n{ex}",
		//		BudgetId = 0,
		//	};
		//}

		//await context.RespondAsync(response);

		var id = await budgetService.CreateNewBudgetAsync(context.Message);
		await context.RespondAsync(new BudgetCreatedDto(id));
	}
}
