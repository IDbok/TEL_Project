using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Customers;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Entities.Projects;


public class Project : AuditableEntity
{
	public int Id { get; set; }
	public long ClassifierId { get; set; }
	public int CustomerId { get; set; }
	public string ProjectName { get; set; } = string.Empty;
	public string ProjectCode { get; set; } = string.Empty;
	public DateTime DateInitiation { get; set; }
	public DateTime? DateCreated { get; set; }

	public Classifier Classifier { get; set; } = null!;
	public Customer Customer { get; set; } = null!;

	public ICollection<Budget> Budgets { get; set; } = [];
	public ICollection<ProjectApproveStatus> ApproveStatuses { get; set; } = [];
	public ICollection<ProjectParameter> Parameters { get; set; } = [];
	public ICollection<ProjectPortfolioCross> PortfolioLinks { get; } = [];
}
