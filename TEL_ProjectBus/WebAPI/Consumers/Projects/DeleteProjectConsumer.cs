using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class DeleteProjectConsumer(ProjectService pService, ILogger<DeleteProjectConsumer> logger) : IConsumer<DeleteProjectCommand>
{
    public async Task Consume(ConsumeContext<DeleteProjectCommand> context)
    {
        if (await pService.DeleteProjectAsync(context.Message.ProjectId, context.CancellationToken))
        {
            await context.RespondAsync(new DeleteProjectResponse
            {
                IsSuccess = true,
                Message = "Project deleted successfully"
            });
        }
        else
        {
            await context.RespondAsync(new DeleteProjectResponse
            {
                IsSuccess = false,
                Message = "Failed to delete project"
            });
        }
    }
}
