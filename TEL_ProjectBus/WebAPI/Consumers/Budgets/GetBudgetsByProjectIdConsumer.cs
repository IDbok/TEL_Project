using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.WebAPI.Messages.Queries.Budgets;

namespace TEL_ProjectBus.WebAPI.Consumers.Budgets;

public class GetBudgetsByProjectIdConsumer(ProjectService projectService, ILogger<GetBudgetsByProjectIdConsumer> logger) : IConsumer<GetBudgetsByProjectIdQuery>
{
        public async Task Consume(ConsumeContext<GetBudgetsByProjectIdQuery> context)
        {
                var query = context.Message;

                var (budgets, totalCount) = await projectService.GetProjectBudgetAsync(
                        query.ProjectId,
                        query.PageNumber,
                        query.PageSize,
                        context.CancellationToken);

                await context.RespondAsync(new GetBudgetsByProjectIdResponse
                {
                        IsSuccess = true,
                        Message = $"Budgets for project {query.ProjectId} retrieved successfully.",
                        Items = budgets,
                        TotalCount = totalCount,
                        PageNumber = query.PageNumber,
                        PageSize = query.PageSize,
                }, context.CancellationToken);
        }
}
