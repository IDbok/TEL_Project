using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TEL_ProjectBus.WebAPI.Common;
using TEL_ProjectBus.WebAPI.Controllers;
using Xunit;

namespace TEL_ProjectBus.Tests;

/// <summary>
/// Проверяем энд-поинт /api/auth/login-test-user:
///  ⸺ отдаёт 200 OK;
///  ⸺ возвращает токен доступа;
public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthTests(CustomWebApplicationFactory factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task LoginTestUser_Admin_Should_Return_Jwt()
    {
        // Arrange
        var payload = new AuthController.TestRoleDto { Role = "Admin" };

        // Act
        var resp = await _client.PostAsJsonAsync(
            "/api/auth/login-test-user",
            payload);

        // Assert
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await resp.Content
                            .ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        body!.data.AccessToken.Should().NotBeNullOrWhiteSpace();
        body.data.UserRoles.Should().Contain("Admin");
    }
}