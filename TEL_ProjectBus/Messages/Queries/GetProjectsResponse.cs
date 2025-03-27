namespace TEL_ProjectBus.Messages.Queries;

public record ProjectDto
{
	public Guid ProjectId { get; init; }
	public string ProjectName { get; init; }
	public string ProjectCode { get; init; }
	public DateTime DateInitiation { get; init; }
	public Guid CustomerId { get; init; }
}

public record GetProjectsResponse
{
	public IEnumerable<ProjectDto> Items { get; init; }
	public int TotalCount { get; init; }
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
}
