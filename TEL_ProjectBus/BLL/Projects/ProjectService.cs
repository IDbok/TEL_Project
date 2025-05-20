using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.BLL.Mappers;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.BLL.Projects;

public class ProjectService(AppDbContext _dbContext)
{
	public async Task<int> CreateNewProjectAsync(ProjectDto project, CancellationToken cancellationToken)
	{
		var newProject = ProjectMapper.ToEntity(project);
		await _dbContext.Projects.AddAsync(newProject, cancellationToken);
		await _dbContext.SaveChangesAsync();
		return newProject.Id;
	}

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
	public async Task<List<BudgetLineDto>> GetProjectBudgetAsync(int projectId, CancellationToken cancellationToken)
	{
		var budgets = await _dbContext.Budgets
						 .AsNoTracking()
						 .Include(b => b.Classifier)
						 .Include(b => b.BudgetGroup)
						 .Where(b => b.ProjectId == projectId)
						 .ToListAsync(cancellationToken);

		if (budgets is null || budgets.Count == 0)
			throw new Exception($"Project with ID {projectId} has no budgets.");

		return BudgetMapper.ToDto<BudgetLineDto>(budgets);
	}
}
