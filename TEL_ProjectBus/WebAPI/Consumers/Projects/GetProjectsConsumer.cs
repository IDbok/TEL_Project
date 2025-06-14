﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.BLL.Mappers;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

namespace TEL_ProjectBus.WebAPI.Consumers.Projects;

public class GetProjectsConsumer(AppDbContext db) : IConsumer<GetProjectsQuery>
{

	public async Task Consume(ConsumeContext<GetProjectsQuery> context)
	{
		var query = context.Message;
		var userId = context.Message.UserId;

		var projectsQuery = await db.Projects // 📌 таблица Projects
			.Include(p => p.Classifier)
			.Include(p => p.Customer)
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

		var totalCount = result.Count;
		var projects = result
			.Skip((query.PageNumber - 1) * query.PageSize)
			.Take(query.PageSize)
			.ToList();

		await context.RespondAsync(new GetProjectsResponse
		{
			//IsSuccess = true,
			//Message = "Projects found.",
			Items = projects.Select(x => x.ToDto<ProjectDto>()),
			TotalCount = totalCount,
			PageNumber = query.PageNumber,
			PageSize = query.PageSize,
		});
	}
}
