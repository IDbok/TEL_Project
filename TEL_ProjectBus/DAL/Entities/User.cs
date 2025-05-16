using Microsoft.AspNetCore.Identity;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities;

public class User : IdentityUser, IHasIdentity<string>
{
	public string Account { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public bool Enabled { get; set; } = true;
	public string? LDAPUserId { get; set; } = null;
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public DateTime? DateChanged { get; set; } = null;
	public Guid? ChangedBy { get; set; } = null;

	public string FullName => $"{FirstName} {LastName}"; // Полное имя пользователя
}
