using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TEL_ProjectBus.WebAPI.Messages.Queries;

namespace TEL_ProjectBus.WebAPI.Controllers;

[Authorize]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]

public class ProjectQueryController : BaseApiController
{
	private readonly IRequestClient<GetProjectsQuery> _getProjectsClient;
	private readonly IRequestClient<GetProjectProfileQuery> _getProjectProfileByIdClient;
	private readonly ILogger<ProjectQueryController> _logger;

	public ProjectQueryController(IRequestClient<GetProjectsQuery> getProjectsClient,
		IRequestClient<GetProjectProfileQuery> getProjectProfileByIdClient,
		ILogger<ProjectQueryController> logger)
	{
		_getProjectsClient = getProjectsClient;
		_getProjectProfileByIdClient = getProjectProfileByIdClient;
		_logger = logger;
	}

	[Authorize]
	[HttpGet("projects")]
	public async Task<IActionResult> GetProjects(
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 20,
		[FromQuery] string projectName = "",
		[FromQuery] string projectCode = "")
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();

		var query = new GetProjectsQuery
		{
			PageNumber = pageNumber,
			PageSize = pageSize,
			ProjectNameFilter = projectName,
			ProjectCodeFilter = projectCode,

			UserId = userId,
		};

		var response = await _getProjectsClient.GetResponse<GetProjectsResponse>(query);

		return ApiOk(response.Message);
	}

	/// <summary>
	/// Возвращает паспорт проекта по указанному идентификатору.
	/// </summary>
	[AllowAnonymous]
	[HttpGet("projects/{id:long}/profile")]
	public async Task<IActionResult> GetProjectProfileById(long id)
	{
		_logger.LogInformation($"[ProjectQueryController] Sending GetBudgetByIdQuery for {id}");

		try
		{
			var response = await _getProjectProfileByIdClient.GetResponse<GetProjectProfileResponse>(
				new GetProjectProfileQuery { ProjectId = id }, timeout: TimeSpan.FromSeconds(30));

			return ApiOk(response.Message);
		}
		catch (RequestTimeoutException ex)
		{
			_logger.LogError(ex, "[ProjectQueryController] Timeout when requesting ProjectProfile by ID {Id}", id);
			return StatusCode(504, "Request Timeout: " + ex.Message);
		}
		catch (RequestFaultException ex) when (ex.Message.Contains("ProjectNotFound"))
		{
			_logger.LogWarning("Project {ProjectId} not found", id);
			return NotFound();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "[ProjectQueryController] Error when requesting ProjectProfile by ID {Id}", id);
			return StatusCode(500, "Internal Server Error: " + ex.Message);
		}
	}
}