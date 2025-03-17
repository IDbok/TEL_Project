using MassTransit;
using TEL_ProjectBus.Messages.Commands;
using TEL_ProjectBus.Messages.Events;

namespace TEL_ProjectBus.Consumers;

public class CreateBudgetItemConsumer : IConsumer<CreateBudgetItemCommand>
{
	public async Task Consume(ConsumeContext<CreateBudgetItemCommand> context)
	{
		var command = context.Message;

		// TODO: Логика сохранения статьи бюджета в БД и генерация ID
		var newBudgetItemId = Guid.NewGuid();

		// После сохранения публикуем событие создания
		await context.Publish(new BudgetItemCreatedEvent
		{
			BudgetItemId = newBudgetItemId,
			BudgetName = command.BudgetName,
			CreatedAt = DateTime.UtcNow
		});
	}
}
