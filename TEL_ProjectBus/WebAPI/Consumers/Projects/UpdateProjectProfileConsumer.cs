using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.WebAPI.Messages.Commands;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class UpdateProjectProfileConsumer : IConsumer<UpdateProjectProfileCommand>
{
	private readonly AppDbContext _dbContext;
	public UpdateProjectProfileConsumer(AppDbContext dbContext) => _dbContext = dbContext;
	public async Task Consume(ConsumeContext<UpdateProjectProfileCommand> context)
	{
		var command = context.Message;

		try
		{
			// Get current project and its last parameter
			await new ProjectService(_dbContext)
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
