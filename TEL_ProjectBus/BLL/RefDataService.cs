using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;

namespace TEL_ProjectBus.BLL;

public class RefDataService(AppDbContext _dbContext)
{
	public async Task<List<BudgetGroupDto>> GetAllBudgetGroupsAsync()
	{
		var budgetGroups = await _dbContext.BudgetGroups
			.AsNoTracking()
			.ToListAsync();
		return budgetGroups.Select(bg => new BudgetGroupDto
		{
			Id = bg.Id,
			Name = bg.Name
		}).ToList();
	}

	public async Task<List<Role>> GetAllRolesAsync()
	{
		return await _dbContext.Roles
			.AsNoTracking()
			.ToListAsync();
	}
}
