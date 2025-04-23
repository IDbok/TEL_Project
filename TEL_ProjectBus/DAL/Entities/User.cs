using Microsoft.AspNetCore.Identity;

namespace TEL_ProjectBus.DAL.Entities;

public class User : IdentityUser
{
	public string Account { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public bool Enabled { get; set; } = true;
	public string? LDAPUserId { get; set; } = null;
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public DateTime? DateChanged { get; set; } = null;
	public Guid? ChangedBy { get; set; } = null;
}
