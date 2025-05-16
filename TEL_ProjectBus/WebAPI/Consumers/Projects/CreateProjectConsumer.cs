using MassTransit;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class CreateProjectConsumer : IConsumer<CreateProjectCommand>
{
	private readonly ILogger<CreateProjectConsumer> _logger;
	private readonly AppDbContext _dbContext;
	public CreateProjectConsumer(ILogger<CreateProjectConsumer> logger, AppDbContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<CreateProjectCommand> context)
	{
		// Simulate project creation logic
		var pService = new ProjectService(_dbContext);
		var newProjectId = await pService.CreateNewProjectAsync(context.Message);

		var response = new CreateProjectResponse
		{
			IsSuccess = true,
			Message = "Project created successfully",
			ProjectId = newProjectId,
		};

		await context.RespondAsync(response);
	}
}
