namespace TEL_ProjectBus.WebAPI.Common;

public record ResponseBase
{
	public required bool IsSuccess { get; init; }
	public required string Message { get; init; }
}
