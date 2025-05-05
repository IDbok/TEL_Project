using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;

namespace TEL_ProjectBus.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
	private readonly UserManager<User> _userManager;
	private readonly IConfiguration _configuration;
	private readonly AppDbContext _context;

	public AuthController(UserManager<User> userManager, IConfiguration configuration, AppDbContext context)
	{
		_userManager = userManager;
		_configuration = configuration;
		_context = context;
	}

	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		var user = await _userManager.FindByNameAsync(loginDto.Username);

		if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
			return Unauthorized();

		var accessToken = GenerateJwtToken(user);
		var refreshToken = GenerateRefreshToken(user.Id);

		await _context.RefreshTokens.AddAsync(refreshToken);
		await _context.SaveChangesAsync();

		return ApiOk(new
		{
			token = accessToken,
			refreshToken = refreshToken.Token
		});
	}

	[AllowAnonymous]
	[HttpPost("login-ad")]
	public async Task<IActionResult> LoginAd()
	{
		var fullUsername = User.Identity?.Name;

		if (string.IsNullOrEmpty(fullUsername) || !User.Identity!.IsAuthenticated)
			return Unauthorized("AD Authentication failed.");

		// Обработка имени: убираем домен
		var username = fullUsername.Contains('\\')
			? fullUsername.Split('\\')[1] // "DOMAIN\\username" -> "username"
			: fullUsername;

		var user = await _userManager.FindByNameAsync(username);

		if (user == null)
		{
			// Новый пользователь, пытаемся получить имя и фамилию из AD
			string firstName = string.Empty;
			string lastName = string.Empty;

			//try
			//{
			//	using (var context = new PrincipalContext(ContextType.Domain))
			//	{
			//		var userPrincipal = UserPrincipal.FindByIdentity(context, username);
			//		if (userPrincipal != null)
			//		{
			//			firstName = userPrincipal.GivenName ?? string.Empty;
			//			lastName = userPrincipal.Surname ?? string.Empty;
			//		}
			//	}
			//}
			//catch (Exception ex)
			//{
			//	// Можно логировать ошибку, если не удалось получить данные из AD
			//	Console.WriteLine($"Ошибка получения данных из AD: {ex.Message}");
			//}

			user = new User
			{
				UserName = username,
				Account = username,
				FirstName = firstName,
				LastName = lastName,
				Enabled = true,
				DateCreated = DateTime.UtcNow
			};

			var createResult = await _userManager.CreateAsync(user);
			if (!createResult.Succeeded)
			{
				return StatusCode(500, "Не удалось создать пользователя");
			}

			// Добавляем роль "User"
			await _userManager.AddToRoleAsync(user, "User");
		}
		else if (!user.Enabled)
		{
			return Unauthorized("Пользователь заблокирован");
		}

		var accessToken = GenerateJwtToken(user);
		var refreshToken = GenerateRefreshToken(user.Id);

		await _context.RefreshTokens.AddAsync(refreshToken);
		await _context.SaveChangesAsync();

		return ApiOk(new
		{
			token = accessToken,
			refreshToken = refreshToken.Token
		});
	}

	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		var username = User.Identity?.Name;
		if (string.IsNullOrEmpty(username))
			return Unauthorized();

		var user = await _userManager.FindByNameAsync(username);
		if (user == null)
			return Unauthorized();

		var tokens = _context.RefreshTokens.Where(rt => rt.UserId == user.Id && !rt.IsRevoked);
		foreach (var t in tokens)
			t.IsRevoked = true;

		await _context.SaveChangesAsync();

		return ApiOk("Logged out successfully");
	}

	[HttpPost("refresh-token")]
	public async Task<IActionResult> Refresh([FromBody] string refreshToken)
	{
		var tokenEntity = await _context.RefreshTokens
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Token == refreshToken && !x.IsRevoked);

		if (tokenEntity == null || tokenEntity.ExpiresAt < DateTime.UtcNow)
			return Unauthorized("Invalid or expired refresh token");

		tokenEntity.IsRevoked = true;

		var newAccessToken = GenerateJwtToken(tokenEntity.User);
		var newRefresh = GenerateRefreshToken(tokenEntity.UserId);

		await _context.RefreshTokens.AddAsync(newRefresh);
		await _context.SaveChangesAsync();

		return ApiOk(new
		{
			token = newAccessToken,
			refreshToken = newRefresh.Token
		});
	}

	private string GenerateJwtToken(User user)
	{
		var roles = _userManager.GetRolesAsync(user).Result;
		var claims = new List<Claim>
	   {
		   new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
		   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
	   };

		var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
		claims.AddRange(roleClaims);

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _configuration["JwtSettings:Issuer"],
			audience: _configuration["JwtSettings:Audience"],
			claims: claims,
			expires: DateTime.Now.AddHours(2),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private RefreshToken GenerateRefreshToken(string userId)
	{
		var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		return new RefreshToken
		{
			Token = token,
			ExpiresAt = DateTime.UtcNow.AddDays(7),
			IsRevoked = false,
			UserId = userId
		};
	}
}

public record LoginDto(string Username, string Password);

