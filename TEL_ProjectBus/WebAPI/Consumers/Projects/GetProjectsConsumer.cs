using MassTransit;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.WebAPI.Messages.Queries;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class GetProjectsConsumer : IConsumer<GetProjectsQuery>
{
	private readonly AppDbContext _dbContext;

	public GetProjectsConsumer(AppDbContext dbContext) => _dbContext = dbContext;

	public async Task Consume(ConsumeContext<GetProjectsQuery> context)
	{
		var query = context.Message;

		var projectsQuery = _dbContext.Projects
			.Where(x =>
				(string.IsNullOrEmpty(query.ProjectNameFilter) || x.ProjectName.Contains(query.ProjectNameFilter)) &&
				(string.IsNullOrEmpty(query.ProjectCodeFilter) || x.ProjectCode.Contains(query.ProjectCodeFilter)));

		var totalCount = await projectsQuery.CountAsync();

		var projects = await projectsQuery
			.Skip((query.PageNumber - 1) * query.PageSize)
			.Take(query.PageSize)
			.ToListAsync();

		await context.RespondAsync(new GetProjectsResponse
		{
			Items = projects.Select(x => new ProjectDto
			{
				Id = x.Id,
				ProjectName = x.ProjectName,
				ProjectCode = x.ProjectCode,
				DateInitiation = x.DateInitiation,

				Phase = new PhaseDto
				{
					PhaseName = "Phase 1"
				}
			}),
			TotalCount = totalCount,
			PageNumber = query.PageNumber,
			PageSize = query.PageSize,
		});
	}
}
