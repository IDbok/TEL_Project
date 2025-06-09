using TEL_ProjectBus.DAL.Entities.Budgets;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Reference;

public class Classifier : AuditableEntity, IHasIdentity<ClassifierKey>
{
	public ClassifierKey Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public ICollection<Budget> Budgets { get; set; } = [];
	public ICollection<Project> Projects { get; set; } = [];
	public ICollection<ProjectParameter> ProjectParameters { get; set; } = [];
}
