using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class Budget : AuditableEntity, IHasIdentity<long>
{
	public long Id { get; set; }
	public int BudgetGroupId { get; set; }
	public int ProjectId { get; set; }
	public string? ERPId { get; set; } // todo: вопрос по архитектуре, как будет работать связь с ERP. В какой момент будер происходить связь с ERP?
	public int? ClassifierId { get; set; }
	public bool VisOnPipeline { get; set; }
	public string Name { get; set; } = string.Empty;
	public int RoleId { get; set; } // todo: вопрос по архитектуре, как будет работать связь с ролями. Что значит эта связь?
	public decimal ManHoursCost { get; set; }
	public string? Description { get; set; }
	public decimal? Amount { get; set; }
	public int Version { get; set; }
	public decimal? Probability { get; set; }

	public DateOnly? DatePlan { get; set; }
	public DateOnly? DateFact { get; set; }

	public string? EC { get; set; }
	public decimal RgpPercent { get; set; }
	public decimal? CPTCCPcs { get; set; }
	public decimal? CalcCPTCCPlan { get; set; }
	public decimal? CPTCCFact { get; set; }
	public decimal? CalcPriceTCPcs { get; set; }
	public decimal? CalcPriceTCC { get; set; }
	public decimal? CalcCV { get; set; }
	public decimal? CalcSV { get; set; }
	public decimal? CalcEV { get; set; }
	public decimal? CalcCPI { get; set; }
	public decimal? CalcSPI { get; set; }

	public BudgetGroup BudgetGroup { get; set; } = null!;
	public Project Project { get; set; } = null!;
	public Classifier? Classifier { get; set; } = null!;
}

