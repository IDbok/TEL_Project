using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class UpdateProjectConsumer(ProjectService pService, ILogger<UpdateProjectConsumer> logger) : IConsumer<UpdateProjectCommand>
{
    public async Task Consume(ConsumeContext<UpdateProjectCommand> context)
    {
        var command = context.Message;

        if (await pService.UpdateProjectAsync(command, context.CancellationToken))
        {
            await context.RespondAsync(new UpdateProjectResponse
            {
                IsSuccess = true,
                Message = "Project updated successfully"
            });
        }
        else
        {
            await context.RespondAsync(new UpdateProjectResponse
            {
                IsSuccess = false,
                Message = "Failed to update project"
            });
        }
    }
}
