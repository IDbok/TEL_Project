using MassTransit;
using TEL_ProjectBus.WebAPI.Messages.Commands;
using TEL_ProjectBus.WebAPI.Messages.Events;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class CreateBudgetItemConsumer : IConsumer<UpdateProjectProfileCommand>
{
	public async Task Consume(ConsumeContext<UpdateProjectProfileCommand> context)
	{
		var command = context.Message;

		// TODO: Логика сохранения статьи бюджета в БД и генерация ID
		var newBudgetItemId = Guid.NewGuid();

		// После сохранения публикуем событие создания
		await context.Publish(new BudgetItemCreatedEvent
		{
			BudgetItemId = newBudgetItemId,
			//BudgetName = command.BudgetName,
			CreatedAt = DateTime.UtcNow
		});
	}
}
