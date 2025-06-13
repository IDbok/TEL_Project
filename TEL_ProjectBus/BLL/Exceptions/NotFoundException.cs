namespace TEL_ProjectBus.BLL.Exceptions;

/// <summary>
/// 404 — сущность не найдена.
/// </summary>
public sealed class NotFoundException : HttpResponseException
{
	public NotFoundException(string message, object? value = null)
		: base(StatusCodes.Status404NotFound, message, value) { }
}
