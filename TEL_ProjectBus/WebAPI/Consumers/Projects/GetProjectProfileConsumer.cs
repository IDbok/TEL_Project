using MassTransit;
using TEL_ProjectBus.BLL.Mappers;
using TEL_ProjectBus.BLL.Projects;
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

		var p = await new ProjectService(_dbContext)
			.GetProjectAsync(ctx.Message.ProjectId, ctx.CancellationToken);

		await ctx.RespondAsync(ProjectProfileMapper.ToDto<GetProjectProfileResponse>(p,p.Parameter));
	}
}

