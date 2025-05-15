using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.BLL.Projects;

public class ProjectService
{
	private readonly AppDbContext _dbContext;
	public ProjectService(AppDbContext db) => _dbContext = db;
	public async Task<Project> GetProjectAsync(int projectId, CancellationToken cancellationToken)
	{
		var p = await _dbContext.Projects
						 .AsNoTracking()
						 .Include(p => p.Customer)
						 .Include(p => p.Parameters)
						 .ThenInclude(p => p.ProjectOwner)
						 .FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);
		if (p is null)
			throw new Exception($"Project with ID {projectId} not found.");
		var lastParameter = p.Parameters
			.OrderByDescending(x => x.ProjectEnd)
			.FirstOrDefault();
		if (lastParameter is null)
			throw new Exception($"Project with ID {projectId} has no parameters.");

		p.Parameter = lastParameter;

		return p;
	}

	public async Task CompareAndUpdateAsync<T>(T current, CancellationToken cancellationToken)
		where T : ProjectProfileDto
	{
		var project = await GetProjectAsync(current.ProjectId, cancellationToken);

		if (project is null)
			throw new Exception($"Project with ID {current.ProjectId} not found.");

		// Compare data with the command

		//await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
