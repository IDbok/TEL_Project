using MassTransit;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.Common.Extensions;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.WebAPI.Messages.Queries;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class GetProjectsConsumer : IConsumer<GetProjectsQuery>
{
	private readonly AppDbContext _dbContext;

	public GetProjectsConsumer(AppDbContext dbContext) => _dbContext = dbContext;

	public async Task Consume(ConsumeContext<GetProjectsQuery> context)
	{
		var query = context.Message;
		var userId = context.Message.UserId;

		//var projectsQuery =
		//	from p in _dbContext.Projects                     /* таблица Projects */      // :contentReference[oaicite:0]{index=0}:contentReference[oaicite:1]{index=1}
		//	join up in
		//		(from pp in _dbContext.ProjectParameters      /* таблица ProjectParameters */ // :contentReference[oaicite:2]{index=2}:contentReference[oaicite:3]{index=3}
		//		 where pp.ProjectOwnerId == query.UserId
		//		 group pp by pp.ProjectId into g
		//		 select new
		//		 {
		//			 ProjectId = g.Key,
		//			 LastChanged = g.Max(x => x.DateChanged)  // свойство наследуется от AuditableEntity // :contentReference[oaicite:4]{index=4}:contentReference[oaicite:5]{index=5}
		//		 })
		//		on p.Id equals up.ProjectId
		//	orderby up.LastChanged descending                /* свежие выше */
		//	select new                                         // проекция по‑желанию
		//	{
		//		Project = p,
		//		LastChanged = up.LastChanged
		//	};

		var projectsQuery = await _dbContext.Projects        // 📌 таблица Projects
			.Select(p => new
			{
				Project = p,

				// Берём *абсолютно* последний ProjectParameter проекта
				LastParam = p.Parameters
							 .OrderByDescending(pp => pp.DateChanged)           // или DateCreated
							 .FirstOrDefault()
			})
			// Оставляем только те проекты, где *последний* параметр принадлежит пользователю
			.Where(x => x.LastParam != null && x.LastParam.ProjectOwnerId == userId)
			.AsNoTracking()
			.ToListAsync();

		// EF не заполняет [NotMapped]-свойства, поэтому делаем это вручную
		foreach (var x in projectsQuery)
			if (x.LastParam != null)
				x.Project.Parameter = x.LastParam;
			else
			{
				throw new Exception($"Project {x.Project.Id} has no parameters");
			}


		// Готовая коллекция проектов
		var result = projectsQuery.Select(x => x.Project).ToList();

		//var projectsQuery = _dbContext.Projects
		//	.Where(x =>
		//		(string.IsNullOrEmpty(query.ProjectNameFilter) || x.ProjectName.Contains(query.ProjectNameFilter)) &&
		//		(string.IsNullOrEmpty(query.ProjectCodeFilter) || x.ProjectCode.Contains(query.ProjectCodeFilter)));

		//var totalCount = await projectsQuery.CountAsync();

		//var projects = await projectsQuery
		//	.Skip((query.PageNumber - 1) * query.PageSize)
		//	.Take(query.PageSize)
		//	.ToListAsync();

		var totalCount = result.Count;
		var projects = result
			.Skip((query.PageNumber - 1) * query.PageSize)
			.Take(query.PageSize)
			.ToList();

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
					PhaseName = x.Parameter.ProjectPhase.GetDescription(),
				}
			}),
			TotalCount = totalCount,
			PageNumber = query.PageNumber,
			PageSize = query.PageSize,
		});
	}
}
