namespace TEL_ProjectBus.WebAPI.Messages.Queries;

public record ProjectDto
{
	//public Guid ProjectId { get; init; }
	public string ProjectName { get; init; } = string.Empty;
	public string ProjectCode { get; init; } = string.Empty;
	public DateTime DateInitiation { get; init; }
	//public Guid CustomerId { get; init; }
	public int ProjectProgress { get; init; } = 0;
	public string ProjectStatus { get; init; } = string.Empty;
	public string ClassifierCode { get; init; } = string.Empty;

	public List<PhaseDto> Phases { get; init; } = new List<PhaseDto>();
}

public record PhaseDto
{
	public string PhaseName { get; init; } = string.Empty;
	public string PhaseStatus { get; init; } = string.Empty;
}

public record GetProjectsResponse
{
	public IEnumerable<ProjectDto> Items { get; init; }
	public int TotalCount { get; init; }
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
}
