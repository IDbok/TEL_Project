namespace TEL_ProjectBus.DAL.Entities;

public class RefreshToken
{
	public int Id { get; set; }
	public string Token { get; set; } = null!;
	public DateTime ExpiresAt { get; set; }
	public bool IsRevoked { get; set; }
	public string UserId { get; set; } = null!;
	public User User { get; set; } = null!;
}

