namespace TEL_ProjectBus.WebAPI.Common;

public record ResponseBase
{
	public bool IsSuccess { get; init; }
	public string Message { get; init; } = string.Empty;
}
