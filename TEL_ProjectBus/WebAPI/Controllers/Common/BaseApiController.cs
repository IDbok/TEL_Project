using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
	protected IActionResult ApiOk<T>(T data)
	{
		var response = new ApiResponse<T>(data);
		return Ok(response);
	}

	protected IActionResult SendResponse<T>(Response<T> resp)
		where T : ResponseBase
	{
		if (resp.Message.IsSuccess)
		{
			return ApiOk(resp.Message);
		}
		else
		{
			return BadRequest(resp.Message);
		}
	}
}
