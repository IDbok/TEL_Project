using Microsoft.AspNetCore.Identity;

namespace TEL_ProjectBus.DAL.Entities;

/// <summary>
/// Расширенная роль:  
/// Name  – тех-код,  
/// DisplayName – «человечное» название (англ. или латиницей),  
/// DisplayNameRu – название на русском,  
/// Description – текстовое описание.
/// </summary>
public class Role : IdentityRole
{
	public string DisplayName { get; set; } = string.Empty;
	public string DisplayNameRu { get; set; } = string.Empty;
	//public string? Description { get; set; }
}