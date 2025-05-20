using MassTransit;
using TEL_ProjectBus.BLL.Mappers;
using TEL_ProjectBus.BLL.Projects;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class GetProjectProfileConsumer(ProjectService pService)
	: IConsumer<GetProjectProfileQuery>
{
	public async Task Consume(ConsumeContext<GetProjectProfileQuery> ctx)
	{

		var p = await pService
			.GetProjectAsync(ctx.Message.ProjectId, ctx.CancellationToken);

		await ctx.RespondAsync(ProjectProfileMapper.ToDto<GetProjectProfileResponse>(p,p.Parameter));
	}
}

