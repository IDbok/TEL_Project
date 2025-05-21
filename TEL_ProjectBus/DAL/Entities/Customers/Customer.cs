using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Customers;

public class Customer : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Address { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public string ContactPerson { get; set; } = string.Empty;
	public string? Uuid { get; set; } = null;

	public ICollection<Project> Projects { get; set; } = [];
	public ICollection<CustomerTeam> Teams { get; set; } = [];
}

