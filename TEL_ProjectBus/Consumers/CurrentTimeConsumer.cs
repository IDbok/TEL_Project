using MassTransit;

namespace TEL_ProjectBus.Consumers;

public class CurrentTimeConsumer(ILogger<CurrentTimeConsumer> logger) : IConsumer<CurrentTime>
{
	public Task Consume(ConsumeContext<CurrentTime> context)
	{
		logger.LogInformation("[{Consumer}] Received: {Message}", nameof(CurrentTimeConsumer), context.Message.Value);
		//Console.WriteLine($"[CurrentTimeConsumer] Received: {context.Message.Value}");
		return Task.CompletedTask;
	}
}
