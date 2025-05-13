using MassTransit;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.Common.Extensions;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.WebAPI.Messages.Queries;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class GetProjectProfileConsumer
	: IConsumer<GetProjectProfileQuery>
{
	private readonly AppDbContext _dbContext;

	public GetProjectProfileConsumer(AppDbContext db) => _dbContext = db;

	public async Task Consume(ConsumeContext<GetProjectProfileQuery> ctx)
	{
		var p = await _dbContext.Projects
						 .AsNoTracking()
						 .Include(p => p.Customer)
						 .Include(p => p.Parameters)
						 .ThenInclude(p => p.ProjectOwner)
						 .FirstOrDefaultAsync(x => x.Id == ctx.Message.ProjectId, ctx.CancellationToken);

		if (p is null)
			throw new Exception($"Project with ID {ctx.Message.ProjectId} not found."); //ProjectNotFoundException(ctx.Message.ProjectId); 

		var lastParameter = p.Parameters
			.OrderByDescending(x => x.ProjectEnd)
			.FirstOrDefault();

		if (lastParameter is null)
			 throw new Exception($"Project with ID {ctx.Message.ProjectId} has no parameters.");

		var currentStage = p.Parameters
			.OrderByDescending(x => x.ProjectEnd)
			.FirstOrDefault(x => x.ProjectStage != null);

		if (currentStage is null)
			throw new Exception($"Project with ID {ctx.Message.ProjectId} has no current stage.");

		await ctx.RespondAsync(new GetProjectProfileResponse
		{
			ProjectId = p.Id,
			Name = p.ProjectName,
			Code = p.ProjectCode,
			Responsible = $"{lastParameter.ProjectOwner.FirstName} {lastParameter.ProjectOwner.LastName}" ,
			Customer = p.Customer.Name,
			ProjectType = "н/д",
			StartDate = lastParameter.ProjectBegin,
			EndDate = lastParameter.ProjectEnd,
			CurrentStage = currentStage.ProjectStage.GetDescription(),

		});
	}
}

