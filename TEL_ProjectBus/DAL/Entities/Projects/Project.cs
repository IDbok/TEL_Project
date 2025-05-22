using System.ComponentModel.DataAnnotations.Schema;
using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Customers;
using TEL_ProjectBus.DAL.Entities.Reference;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;


public class Project : AuditableEntity, IHasIdentity<int>, IHasClassifier
{
	public int Id { get; set; }
	public ClassifierKey ClassifierId { get; set; }
	public int CustomerId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public DateTime DateInitiation { get; set; }
	public DateTime? DateCreated { get; set; }

	public Classifier Classifier { get; set; } = null!;
	public Customer Customer { get; set; } = null!;

	[NotMapped] public ProjectParameter Parameter { get; set; } = null!;

	public ICollection<Budget> Budgets { get; set; } = [];
	public ICollection<ProjectApproveStatus> ApproveStatuses { get; set; } = [];
	public ICollection<ProjectParameter> Parameters { get; set; } = [];
	public ICollection<ProjectPortfolioCross> PortfolioLinks { get; } = [];
}
