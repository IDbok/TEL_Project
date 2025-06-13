using TEL_ProjectBus.BLL.DTOs;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

public record GetProjectsResponse
{
	public IEnumerable<ProjectDto> Items { get; init; } = [];
	public int TotalCount { get; init; }
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
}


