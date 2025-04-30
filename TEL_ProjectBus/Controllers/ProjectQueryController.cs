using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.Messages.Queries;

namespace TEL_ProjectBus.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectQueryController : BaseApiController
{
	private readonly IRequestClient<GetProjectsQuery> _getProjectsClient;

	public ProjectQueryController(IRequestClient<GetProjectsQuery> getProjectsClient)
	{
		_getProjectsClient = getProjectsClient;
	}

	[HttpGet("projects")]
	public async Task<IActionResult> GetProjects(
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 20,
		[FromQuery] string projectName = "",
		[FromQuery] string projectCode = "")
	{
		var query = new GetProjectsQuery
		{
			PageNumber = pageNumber,
			PageSize = pageSize,
			ProjectNameFilter = projectName,
			ProjectCodeFilter = projectCode
		};

		var response = await _getProjectsClient.GetResponse<GetProjectsResponse>(query);

		return ApiOk(response.Message);
	}
}