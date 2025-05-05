using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class BudgetApprove : AuditableEntity
{
	public Guid Id { get; set; }
	public Guid BudgetId { get; set; }
	public int? RoleId { get; set; } // Если роли вынесены в отдельную справоч. табл.
	public Guid? UserId { get; set; }
	public Guid ApproveStatusId { get; set; }
	public bool Approved { get; set; }
	public string? Comment { get; set; }

	public Budget Budget { get; set; } = null!;
	public ApproveStatus ApproveStatus { get; set; } = null!;
	public User? User { get; set; }
}
