using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.Common;

namespace TEL_ProjectBus.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
	protected IActionResult ApiOk<T>(T data)
	{
		var response = new ApiResponse<T>(data);
		return Ok(response);
	}
}
