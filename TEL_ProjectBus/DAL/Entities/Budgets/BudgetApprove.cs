using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Entities.Budgets;

public class BudgetApprove : AuditableEntity
{
	public long Id { get; set; }
	public long BudgetId { get; set; }
	public int RoleId { get; set; } // todo: проверить как будет работать связь с ролями
	public string? ApproverId { get; set; }
	public Guid ApproveStatusId { get; set; }
	public bool Approved { get; set; }
	public string? Comment { get; set; }

	public Budget Budget { get; set; } = null!;
	public User? Approver { get; set; }
}
