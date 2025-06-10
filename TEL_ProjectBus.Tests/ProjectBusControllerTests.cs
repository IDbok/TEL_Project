using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.WebAPI.Common;
using TEL_ProjectBus.WebAPI.Controllers;
using TEL_ProjectBus.WebAPI.Messages.Commands.Projects;
using Xunit;

namespace TEL_ProjectBus.Tests;
public class ProjectBusControllerTests : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _client;

	public ProjectBusControllerTests(CustomWebApplicationFactory factory)
		=> _client = factory.CreateClient();

	private async Task AuthenticateAsync()
	{
		var resp = await _client.PostAsJsonAsync("/api/auth/login-test-user", new AuthController.TestRoleDto { Role = "Admin" });
		var login = await resp.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.data.AccessToken);
	}

	private static CreateProjectCommand BuildCreateCommand()
	{
		return new CreateProjectCommand
		{
			Name = "TestProject",
			Code = Guid.NewGuid().ToString("N").Substring(0, 8),
			Classifier = new ClassifierDto { Id = (ClassifierKey)1, Code = "Classifier 1" },
			Customer = new CustomerDto { Id = 1, Name = "Клиент №1", CompanyName = "Компания 1", ContactPerson = "Контакт 1" },
			DateInitiation = DateTime.UtcNow,
			DateCreated = DateTime.UtcNow,
			ChangedByUserId = "00000000-0000-0000-0000-000000000001",
			DateChanged = DateTime.UtcNow,
			BudgetLines = []
		};
	}

	private static UpdateProjectCommand BuildUpdateCommand(int id)
	{
		return new UpdateProjectCommand
		{
			Id = id,
			Name = "Updated",
			Code = "UPD",
			Classifier = new ClassifierDto { Id = (ClassifierKey)1, Code = "Classifier 1" },
			Customer = new CustomerDto { Id = 1, Name = "Клиент №1", CompanyName = "Компания 1", ContactPerson = "Контакт 1" },
			DateInitiation = DateTime.UtcNow,
			DateCreated = DateTime.UtcNow,
			ChangedByUserId = "00000000-0000-0000-0000-000000000001",
			DateChanged = DateTime.UtcNow,
			BudgetLines = []
		};
	}

	[Fact]
	public async Task UpdateProjectProfile_Should_Return_Accepted()
	{
		var command = new UpdateProjectProfileCommand
		{
			ProjectId = 1,
			Name = "Proj",
			Code = "P1",
			PreparedBy = "Tester",
			Department = "Dep",
			Customer = "Cust",
			ResponsibleFullName = "Resp",
			ContactPhone = "123",
			DateCreated = DateTime.UtcNow,
			GoalsAndRequirements = "G",
			CustomerNeeds = "N",
			SuccessCriteria = "S",
			HighLevelRisks = "H",
			ScheduleAndBudget = "B",
			AssumptionsAndConstraints = "A",
			ConfidenceRequirements = "C",
			ProjectManager = "M"
		};

		var resp = await _client.PutAsJsonAsync("/api/ProjectBus/projects/update-profile", command);
		resp.StatusCode.Should().Be(HttpStatusCode.Accepted);
	}

	[Fact]
	public async Task Create_Update_Delete_Project_Flow()
	{
		var createCommand = BuildCreateCommand();
		var createResp = await _client.PostAsJsonAsync("/api/ProjectBus/projects/create", createCommand);
		createResp.StatusCode.Should().Be(HttpStatusCode.Accepted);
		var createBody = await createResp.Content.ReadFromJsonAsync<ApiResponse<CreateProjectResponse>>();
		var newId = createBody!.data.ProjectId;
		newId.Should().BeGreaterThan(0);

		await AuthenticateAsync();
		var updateCommand = BuildUpdateCommand(newId);
		var updateResp = await _client.PutAsJsonAsync($"/api/ProjectBus/projects/{newId}/update", updateCommand);
		updateResp.StatusCode.Should().Be(HttpStatusCode.OK);

		var deleteResp = await _client.DeleteAsync($"/api/ProjectBus/projects/{newId}/delete");
		deleteResp.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task UpdateProject_Should_Return_Unauthorized_When_No_Token()
	{
		var updateCommand = BuildUpdateCommand(1);
		var resp = await _client.PutAsJsonAsync("/api/ProjectBus/projects/1/update", updateCommand);
		resp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
	}
}