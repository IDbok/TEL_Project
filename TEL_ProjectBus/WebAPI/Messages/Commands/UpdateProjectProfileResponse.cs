namespace TEL_ProjectBus.WebAPI.Messages.Commands;

public record UpdateProjectProfileResponse
{
	public bool IsSuccess { get; init; }
	public string Message { get; init; } = string.Empty;
}
