using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Entities.Customers;

public class Customer : AuditableEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Address { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public string ContactPerson { get; set; } = string.Empty;
	public DateTime? DateCreated { get; set; }

	public ICollection<Project> Projects { get; set; } = [];
	public ICollection<CustomerTeam> Teams { get; set; } = [];
}

