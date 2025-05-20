using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class UpdateProjectProfileConsumer(ProjectService pService) : IConsumer<UpdateProjectProfileCommand>
{
	public async Task Consume(ConsumeContext<UpdateProjectProfileCommand> context)
	{
		var command = context.Message;

		try
		{
			await pService
				.CompareAndUpdateAsync(command, context.CancellationToken);

			await context.RespondAsync(new UpdateProjectProfileResponse
			{
				IsSuccess = true,
				Message = "Project profile updated successfully."
			});
		}
		catch (Exception ex)
		{
			await context.RespondAsync(new UpdateProjectProfileResponse
			{
				IsSuccess = false,
				Message = $"Error updating project profile: {ex.Message}"
			});
		}
	}
}
