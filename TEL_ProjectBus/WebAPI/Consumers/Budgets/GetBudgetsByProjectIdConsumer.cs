using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class GetBudgetsByProjectIdConsumer(ProjectService projectService, ILogger<GetBudgetsByProjectIdConsumer> logger) : IConsumer<GetBudgetsByProjectIdQuery>
{
	public async Task Consume(ConsumeContext<GetBudgetsByProjectIdQuery> context)
	{
		var query = context.Message;
		var projectId = query.ProjectId;
		var budgets = await projectService.GetProjectBudgetAsync(context.Message.ProjectId, context.CancellationToken);
		await context.RespondAsync(new GetBudgetsByProjectIdResponse
		{
			Budgets = budgets,
		});
	}
}
