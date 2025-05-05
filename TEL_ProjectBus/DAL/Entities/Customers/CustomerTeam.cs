namespace TEL_ProjectBus.DAL.Entities.Customers;

public class CustomerTeam
{
	public Guid Id { get; set; }
	public Guid CustomerId { get; set; }

	public string CustomerMemberRole { get; set; } = string.Empty;
	public string CustomerTeamMemberName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;

	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public DateTime DateChanged { get; set; } = DateTime.UtcNow;
	public Guid ChangedBy { get; set; }

	public Customer Customer { get; set; } = null!;
}
