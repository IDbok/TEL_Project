using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class CreateProjectConsumer(ProjectService pService, ILogger<CreateProjectConsumer> logger) : IConsumer<CreateProjectCommand>
{
	public async Task Consume(ConsumeContext<CreateProjectCommand> context)
	{
		var newProjectId = await pService.CreateNewProjectAsync(context.Message, context.CancellationToken);

		var response = new CreateProjectResponse
		{
			//IsSuccess = true,
			//Message = "Project created successfully",
			ProjectId = newProjectId,
		};

		await context.RespondAsync(response);
	}
}
