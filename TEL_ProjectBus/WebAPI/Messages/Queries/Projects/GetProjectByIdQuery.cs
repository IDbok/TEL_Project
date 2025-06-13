namespace TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

public record GetProjectByIdQuery
{
	public int ProjectId { get; init; }
	public string UserId { get; init; } = "";
}
