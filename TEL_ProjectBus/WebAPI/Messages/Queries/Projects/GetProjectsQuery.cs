namespace TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

public record GetProjectsQuery
{
	public int PageNumber { get; init; } = 1;
	public int PageSize { get; init; } = 20;
	public string ProjectNameFilter { get; init; } = "";
	public string ProjectCodeFilter { get; init; } = "";
	public string UserId { get; init; } = "";
}