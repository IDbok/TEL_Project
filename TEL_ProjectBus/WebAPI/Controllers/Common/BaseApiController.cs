using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TEL_ProjectBus.BLL.Exceptions;
using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
	protected IActionResult ApiOk<T>(T data)
	{
		//var response = new ApiResponse<T>(data);
		//return Ok(response);
		return Ok(data);
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


	protected IActionResult MapFaultToHttp(RequestFaultException faultEx)
	{
		var exInfo = faultEx.Fault.Exceptions?.FirstOrDefault();
		if (exInfo is null)                    // совсем странно, но вдруг
			return StatusCode(500, "Unknown fault");

		// Пытаемся понять тип исключения (полное имя в ExceptionType)
		var typeName = exInfo.ExceptionType;   // например: "MyApp.Exceptions.NotFoundException"

		// Можно хранить словарь, но для пары типов switch вполне ок
		return typeName.EndsWith(nameof(NotFoundException))
					? NotFound(exInfo.Message)
					: StatusCode(500, exInfo.Message);
		//? NotFound(new ApiResponse<string>(exInfo.Message))
		//: StatusCode(500, new ApiResponse<string>(exInfo.Message));

	}
}
