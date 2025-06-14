﻿using Microsoft.EntityFrameworkCore;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.BLL.Mappers;
using TEL_ProjectBus.DAL.DbContext;

namespace TEL_ProjectBus.BLL.Budgets;

public class BudgetService(AppDbContext _dbContext)
{
	public async Task<long> CreateNewBudgetAsync<T>(T budget, CancellationToken ct) // todo: передать пользователя и присвоить changedByid
		where T : BudgetLineDto
	{
		ArgumentNullException.ThrowIfNull(budget);

		var newBudget = BudgetMapper.ToEntity(budget);

		await _dbContext.Budgets.AddAsync(newBudget, ct);
		await _dbContext.SaveChangesAsync(ct);

		return newBudget.Id;
	}

	public async Task<T?> GetBudgetByIdAsync<T>(long budgetId, CancellationToken cancellationToken)
		where T : BudgetLineDto, new()
	{
		var budget = await _dbContext.Budgets
			.Include(b => b.BudgetGroup)
			.FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);
		if (budget == null)
			return null;
		return budget.ToDto<T>();
	}

	public async Task<bool> UpdateBudgetAsync<T>(T budget, CancellationToken cancellationToken)
		where T : BudgetLineDto
	{
		if (budget == null)
			throw new ArgumentNullException(nameof(budget));
		if (budget.Id == 0)
			throw new ArgumentException("Budget ID cannot be 0.", nameof(budget));

		var existingBudget = await _dbContext.Budgets
			.FirstOrDefaultAsync(b => b.Id == budget.Id, cancellationToken);
		if (existingBudget == null)
			return false;
		existingBudget.Name = budget.Name;
		existingBudget.Description = budget.Description;
		existingBudget.Amount = budget.Amount;
		existingBudget.Version = budget.Version;
		existingBudget.Probability = budget.Probability;
		existingBudget.DatePlan = budget.DatePlan;
		existingBudget.DateFact = budget.DateFact;
		existingBudget.EC = budget.EC;
		existingBudget.RgpPercent = budget.RgpPercent;
		existingBudget.CPTCCPcs = budget.CPTCCPcs;
		existingBudget.CalcCPTCCPlan = budget.CalcCPTCCPlan;
		existingBudget.CPTCCFact = budget.CPTCCFact;
		existingBudget.CalcPriceTCPcs = budget.CalcPriceTCPcs;
		existingBudget.CalcPriceTCC = budget.CalcPriceTCC;
		existingBudget.CalcCV = budget.CalcCV;
		existingBudget.CalcSV = budget.CalcSV;
		existingBudget.CalcEV = budget.CalcEV;
		existingBudget.CalcCPI = budget.CalcCPI;
		existingBudget.CalcSPI = budget.CalcSPI;
		await _dbContext.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> UpdateBudgetAutoAsync<T>(T dto, CancellationToken cancellationToken)
		where T : BudgetLineDto
	{
		if (dto is null) throw new ArgumentNullException(nameof(dto));
		if (dto.Id == 0) throw new ArgumentException("Id == 0");

		var entity = await _dbContext.Budgets
									 .FirstOrDefaultAsync(b => b.Id == dto.Id, cancellationToken);
		if (entity is null) return false;

		_dbContext.Entry(entity).CurrentValues.SetValues(dto);
		entity.BudgetGroupId = dto.BudgetGroup.Id; // todo: может группу менять отдельным методом?

		await _dbContext.SaveChangesAsync(cancellationToken);
		return true;
	}


	public async Task<bool> DeleteBudgetAsync(long budgetId, CancellationToken cancellationToken)
	{
		var budget = await _dbContext.Budgets
			.FirstOrDefaultAsync(b => b.Id == budgetId, cancellationToken);
		if (budget == null)
			return false;
		_dbContext.Budgets.Remove(budget);
		await _dbContext.SaveChangesAsync(cancellationToken);
		return true;
	}

}
