namespace TEL_ProjectBus.WebAPI.Messages.Queries;

public record ProjectDto
{
	public int Id { get; init; }
	public string ProjectName { get; init; } = string.Empty;
	public string ProjectCode { get; init; } = string.Empty;
	public DateTime DateInitiation { get; init; }

	public string ClassifierCode { get; init; } = string.Empty;

	public List<ProjectParameterDto> Parameters { get; init; } = [];
}

public record PhaseDto
{
	public int Id { get; init; }
	public string PhaseName { get; init; } = string.Empty;
}

public record ProjectParameterDto
{
	public long Id { get; init; }
	public string? Description { get; init; }
	public DateTime ProjectBegin { get; init; }
	public DateTime ProjectEnd { get; init; }
	public PhaseDto ProjectPhase { get; init; } = null!;
}

public record GetProjectsResponse
{
	public IEnumerable<ProjectDto> Items { get; init; }
	public int TotalCount { get; init; }
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
}
