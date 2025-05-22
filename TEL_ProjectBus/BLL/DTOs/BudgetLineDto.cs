using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.BLL.DTOs;

public record BudgetLineDto 
{
	public long Id { get; set; }
	public BudgetGroupDto BudgetGroup { get; init; } = new();
	public int ProjectId { get; init; }
	public string? ERPId { get; init;}
	public ClassifierKey? ClassifierId { get; init; }
	public bool VisOnPipeline { get; init; }
	public string Name { get; init; } = string.Empty;
	public int RoleId { get; init; }
	public decimal ManHoursCost { get; init; }
	public string? Description { get; init; }
	public decimal? Amount { get; init; }
	public int Version { get; init; }
	public decimal? Probability { get; init; }

	public DateTime? DatePlan { get; init; }
	public DateTime? DateFact { get; init; }

	public string? EC { get; init; }
	public decimal RgpPercent { get; init; }
	public decimal? CPTCCPcs { get; init; }
	public decimal? CalcCPTCCPlan { get; init; }
	public decimal? CPTCCFact { get; init; }
	public decimal? CalcPriceTCPcs { get; init; }
	public decimal? CalcPriceTCC { get; init; }
	public decimal? CalcCV { get; init; }
	public decimal? CalcSV { get; init; }
	public decimal? CalcEV { get; init; }
	public decimal? CalcCPI { get; init; }
	public decimal? CalcSPI { get; init; }

	public void SetBudgetId(long id)
	{
		Id = id;
	}
}
