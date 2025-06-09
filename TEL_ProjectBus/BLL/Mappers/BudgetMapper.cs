using AutoMapper;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.DAL.Entities.Budgets;

namespace TEL_ProjectBus.BLL.Mappers;

public static class BudgetMapper
{
	public static T ToDto<T>(this Budget bl)
		where T : BudgetLineDto, new()
	{
		if (bl == null) throw new ArgumentNullException(nameof(bl));
		return new T
		{
			Id = bl.Id,
			Name = bl.Name,
			ProjectId = bl.ProjectId,
			ERPId = bl.ERPId,
			ClassifierId = bl.ClassifierId,
			VisOnPipeline = bl.VisOnPipeline,
			RoleId = bl.RoleId,
			ManHoursCost = bl.ManHoursCost,
			Description = bl.Description,
			Amount = bl.Amount,
			Version = bl.Version,
			Probability = bl.Probability,
			DatePlan = bl.DatePlan,
			DateFact = bl.DateFact,
			EC = bl.EC,
			RgpPercent = bl.RgpPercent,
			CPTCCPcs = bl.CPTCCPcs,
			CalcCPTCCPlan = bl.CalcCPTCCPlan,
			CPTCCFact = bl.CPTCCFact,
			CalcPriceTCPcs = bl.CalcPriceTCPcs,
			CalcPriceTCC = bl.CalcPriceTCC,
			CalcCV = bl.CalcCV,
			CalcSV = bl.CalcSV,
			CalcEV = bl.CalcEV,
			CalcCPI = bl.CalcCPI,
			CalcSPI = bl.CalcSPI,
			BudgetGroup = new BudgetGroupDto
			{
				Id = bl.BudgetGroupId,
				Name = bl.BudgetGroup.Name,
			},
		};
	}

	public static List<T> ToDto<T>(this ICollection<Budget> bls)
		where T : BudgetLineDto, new()
	{
		if (bls == null) throw new ArgumentNullException(nameof(bls));

		var result = new List<T>();

		foreach ( var b in bls )
		{
			var dto = b.ToDto<T>();
			result.Add(dto);
		}

		return result;
	}

	public static Budget ToEntity<T>(this T bl) 
		where T : BudgetLineDto
	{
		if (bl == null) throw new ArgumentNullException(nameof(bl));
		return new Budget
		{
			Id = bl.Id,
            Name = bl.Name,
			ProjectId = bl.ProjectId,
			ERPId = bl.ERPId,
			ClassifierId = bl.ClassifierId,
			VisOnPipeline = bl.VisOnPipeline,
			RoleId = bl.RoleId,
			ManHoursCost = bl.ManHoursCost,
			Description = bl.Description,
			Amount = bl.Amount,
			Version = bl.Version,
			Probability = bl.Probability,
			DatePlan = bl.DatePlan,
			DateFact = bl.DateFact,
			EC = bl.EC,
			RgpPercent = bl.RgpPercent,
			CPTCCPcs = bl.CPTCCPcs,
			CalcCPTCCPlan = bl.CalcCPTCCPlan,
			CPTCCFact = bl.CPTCCFact,
			CalcPriceTCPcs = bl.CalcPriceTCPcs,
			CalcPriceTCC = bl.CalcPriceTCC,
			CalcCV = bl.CalcCV,
			CalcSV = bl.CalcSV,
			CalcEV = bl.CalcEV,
			CalcCPI = bl.CalcCPI,
			CalcSPI = bl.CalcSPI,

			BudgetGroupId = bl.BudgetGroup.Id,

			ChangedByUserId = "00000000-0000-0000-0000-000000000002", // pm // todo: get from user

		};
	}

	public static List<Budget> ToEntity<T>(this List<T> entities)
		where T : BudgetLineDto
	{
		if (entities == null || entities.Count == 0) throw new ArgumentNullException(nameof(entities));

		return entities.Select(e => ToEntity(e)).ToList(); ;
	}
}
