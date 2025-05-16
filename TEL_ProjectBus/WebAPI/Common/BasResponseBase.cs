namespace TEL_ProjectBus.WebAPI.Common;

public record BasResponseBase
{
	public bool IsSuccess { get; init; }
	public string Message { get; init; } = string.Empty;
}
