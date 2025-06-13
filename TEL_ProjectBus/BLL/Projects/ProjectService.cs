using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.BLL.Exceptions;
using TEL_ProjectBus.BLL.Mappers;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

namespace TEL_ProjectBus.BLL.Projects;

public class ProjectService(AppDbContext _dbContext)
{
	public async Task<int> CreateNewProjectAsync(ProjectDto project, CancellationToken cancellationToken)
    {
        var newProject = ProjectMapper.ToEntity(project);

        await _dbContext.Projects.AddAsync(newProject, cancellationToken);

        await AddBudgetLinesToContext(project.BudgetLines, newProject, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return newProject.Id;
    }

    private async Task AddBudgetLinesToContext(List<BudgetLineDto> budgetLines, Project newProject, 
        CancellationToken cancellationToken)
    {
        if (budgetLines != null && budgetLines.Count > 0)
        {
            var newBudgetLines = BudgetMapper.ToEntity(budgetLines);
            foreach (var budgetLine in newBudgetLines)
            {
                budgetLine.ProjectId = 0;
                budgetLine.Project = newProject;
            }

            await _dbContext.Budgets.AddRangeAsync(newBudgetLines, cancellationToken);
        }
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

        p.Parameters.Clear();

		return p;
	}

    [Obsolete("This method does not work.")]
	public async Task CompareAndUpdateAsync<T>(T current, CancellationToken cancellationToken)
		where T : ProjectProfileDto
	{
		var project = await GetProjectAsync(current.ProjectId, cancellationToken);

		if (project is null)
			throw new Exception($"Project with ID {current.ProjectId} not found.");

		// Compare data with the command

		//await _dbContext.SaveChangesAsync(cancellationToken);
	}

    public async Task<(List<BudgetLineDto> Budgets, int TotalCount)> GetProjectBudgetAsync(
            int projectId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
    {
		// проверим, существует ли проект
		var projectExists = await _dbContext.Projects
			.AnyAsync(p => p.Id == projectId, cancellationToken);
		if (!projectExists)
			throw new NotFoundException($"Project with ID {projectId} not found.");

		var query = _dbContext.Budgets
                                .AsNoTracking()
                                .Include(b => b.Classifier)
                                .Include(b => b.BudgetGroup)
                                .Where(b => b.ProjectId == projectId);


        var totalCount = await query.CountAsync(cancellationToken);

        var budgets = await query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken);

        return (BudgetMapper.ToDto<BudgetLineDto>(budgets), totalCount);
    }

    public async Task<bool> UpdateProjectAsync(UpdateProjectCommand project, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Projects
            .Include(p => p.Classifier)
			.Include(p => p.Customer)
			.FirstOrDefaultAsync(p => p.Id == project.Id, cancellationToken);
        if (entity is null)
                return false;

        entity.Name = project.Name;
        entity.Code = project.Code;

        if (project.Classifier is not null && project.Classifier.Id != entity.ClassifierId)
        {
            var classifier = await _dbContext.Classifiers
				.FirstOrDefaultAsync(c => c.Id == project.Classifier.Id, cancellationToken);
			if (classifier is null)
				// todo: invalid data exception
				throw new Exception($"Classifier with ID {project.Classifier.Id} not found.");
			entity.Classifier = classifier;
        }

        if (project.Customer is not null && project.Customer.Id != entity.CustomerId)
        {
			var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == project.Customer.Id, cancellationToken);
			if (customer is null)
                throw new Exception($"Customer with ID {project.Customer.Id} not found.");
			entity.Customer = customer;
		}

		// todo: можно ли менять дату создания и инициализации при обновлении?
		entity.DateInitiation = project.DateInitiation;
        entity.DateCreated = project.DateCreated; 

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteProjectAsync(int projectId, CancellationToken cancellationToken)
    {
            var entity = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
            if (entity is null)
                    return false;

            _dbContext.Projects.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
    }
}
