using MassTransit;
using TEL_ProjectBus.WebAPI.Messages.Commands;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class UpdateProjectProfileItemConsumer : IConsumer<UpdateProjectProfileCommand>
{
	public async Task Consume(ConsumeContext<UpdateProjectProfileCommand> context)
	{
		var command = context.Message;

		// Здесь вы можете добавить логику для обновления профиля проекта


		await context.RespondAsync(new UpdateProjectProfileResponse
		{
			IsSuccess = true,
			Message = "Project profile updated successfully."
		});
	}
}
