using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using TEL_ProjectBus.DAL.DbContext;
using TEL_ProjectBus.DAL.Entities;
using TEL_ProjectBus.WebAPI.Controllers.Common;

namespace TEL_ProjectBus.WebAPI.Controllers;

[Authorize]
public class AuthController(UserManager<User> _userManager, IConfiguration _configuration, AppDbContext _context) : BaseApiController
{
	public record TestRoleDto
	{
		public string Role { get; init; } = "Admin";
	}
	/// <summary>
	/// Авторизация пользователя под конкретную роль.
	/// Доступные роли: Admin, PM, PL, SD, SM, SiM, PE, RO, LC, DM, IM, AM, SSD
	/// </summary>
	[AllowAnonymous]
	[HttpPost("login-test-user")]
	public async Task<IActionResult> LoginTestUser([FromBody] TestRoleDto request)
	{
		if (!DbInitializer.testRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
			return Unauthorized($"Invalid role: {request.Role}");

		var userName = $"{request.Role.ToLower()}_test";
		var password = DbInitializer.testUserPassword;

		var user = await _userManager.FindByNameAsync(userName);

		if (user == null || !await _userManager.CheckPasswordAsync(user, password))
			return Unauthorized();

		var resoinse = await GetLoginResponseAsync(user);
		await _context.RefreshTokens.AddAsync(resoinse.RefreshToken);
		await _context.SaveChangesAsync();
		return ApiOk(resoinse);
	}

	/// <summary>
	/// Выполняет вход пользователя с использованием предоставленных учетных данных.
	/// В случае успешной авторизации генерирует токены доступа и обновления, и возвращает их клиенту.
	/// </summary>
	/// <param name="loginDto">Объект, содержащий имя пользователя и пароль.</param>
	/// <returns>Возвращает токен доступа и токен обновления, если аутентификация прошла успешно. 
	/// В случае неудачи — статус 401 (Unauthorized).</returns>
	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		var user = await _userManager.FindByNameAsync(loginDto.Username);

		if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
			return Unauthorized();

		var resoinse = await GetLoginResponseAsync(user);
		await _context.RefreshTokens.AddAsync(resoinse.RefreshToken);
		await _context.SaveChangesAsync();
		return ApiOk(resoinse);
	}

	/// <summary>
	/// Выполняет аутентификацию пользователя через Active Directory (AD).
	/// Если пользователь существует, генерирует токены доступа и обновления, и возвращает их. 
	/// В случае отсутствия пользователя в системе, создает нового пользователя и добавляет его в базу данных.
	/// </summary>
	/// <returns>Возвращает токен доступа и токен обновления, если аутентификация прошла успешно. 
	/// В случае неудачи — статус 401 (Unauthorized).</returns>
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

		var resoinse = await GetLoginResponseAsync(user);
		await _context.RefreshTokens.AddAsync(resoinse.RefreshToken);
		await _context.SaveChangesAsync();
		return ApiOk(resoinse);
	}

	/// <summary>
	/// Выполняет выход пользователя из системы, аннулируя все активные refresh токены.
	/// </summary>
	/// <returns>Возвращает статус 200 (OK), если выход из системы выполнен успешно. 
	/// В случае неудачи — статус 401 (Unauthorized).</returns>
	[HttpPost("logout")] // todo: проверить работу метода. По идее, токен должен перестать действовать. А он всё ещё работает
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

	/// <summary>
	/// Обновляет токен доступа с использованием refresh токена.
	/// Если refresh токен действителен, генерирует новый access токен и новый refresh токен.
	/// </summary>
	/// <param name="refreshToken">Refresh токен для обновления.</param>
	/// <returns>Возвращает новый токен доступа и новый refresh токен, если refresh токен действителен.
	/// В случае ошибки — статус 401 (Unauthorized).</returns>
	[HttpPost("refresh-token")]
	public async Task<IActionResult> Refresh([FromBody] string refreshToken)
	{
		var tokenEntity = await _context.RefreshTokens
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Token == refreshToken && !x.IsRevoked);

		if (tokenEntity == null || tokenEntity.ExpiresAt < DateTime.UtcNow)
			return Unauthorized("Invalid or expired refresh token");

		tokenEntity.IsRevoked = true;

		var resoinse = await GetLoginResponseAsync(tokenEntity.User);
		await _context.RefreshTokens.AddAsync(resoinse.RefreshToken);
		await _context.SaveChangesAsync();
		return ApiOk(resoinse);
	}

	private async Task<LoginResponse> GetLoginResponseAsync(User user)
	{
		var accessToken = GenerateJwtToken(user);
		var refreshToken = GenerateRefreshToken(user.Id);
		var roles = await _userManager.GetRolesAsync(user);

		return new LoginResponse
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken,
			UserRoles = roles.ToArray()
		};
	}

	private string GenerateJwtToken(User user)
	{
		var roles = _userManager.GetRolesAsync(user).Result;
		var claims = new List<Claim>
		{
		   new Claim(JwtRegisteredClaimNames.Sub, user.Id),
		   new Claim(ClaimTypes.Name, user.UserName),
		   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};
		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _configuration["JwtSettings:Issuer"],
			audience: _configuration["JwtSettings:Audience"],
			claims: claims,
			expires: DateTime.Now.AddHours(9),
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
public record LoginResponse
{
	public string AccessToken { get; init; } = default!;
	[JsonIgnore]
	public RefreshToken? RefreshToken { get; init; } = default!;
	public string? RefreshTokenString => RefreshToken?.Token;
	public string? UserFullname => RefreshToken?.User.FullName;
	public string[] UserRoles { get; init; } = default!;

}

