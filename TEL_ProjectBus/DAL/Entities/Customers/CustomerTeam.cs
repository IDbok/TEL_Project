using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Entities.Customers;

public class CustomerTeam : AuditableEntity
{
	public Guid Id { get; set; }
	public int CustomerId { get; set; }

	public string CustomerMemberRole { get; set; } = string.Empty;
	public string CustomerTeamMemberName { get; set; } = string.Empty;
	public string? Phone { get; set; }
	public string? Email { get; set; } 

	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	public Customer Customer { get; set; } = null!;
}
