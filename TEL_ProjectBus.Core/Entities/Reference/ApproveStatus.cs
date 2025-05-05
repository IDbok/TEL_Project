using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Entities.Reference;

public class ApproveStatus
{
	public Guid Id { get; set; }
	public string StatusName { get; set; } = string.Empty;
	public ICollection<ProjectApproveStatus> ProjectApproveStatuses { get; set; } = [];
	public ICollection<BudgetApprove> BudgetApproves { get; set; } = [];
}
