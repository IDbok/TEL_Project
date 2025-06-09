using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

//public record ProjectDto
//{
//	public int Id { get; init; }
//	public string ProjectName { get; init; } = string.Empty;
//	public string ProjectCode { get; init; } = string.Empty;
//	public string ClassifierCode { get; init; } = string.Empty;
//	public DateTime DateInitiation { get; init; }
//	public int Progress { get; init; }
//	public PhaseDto Phase { get; init; } = null!;
//}

//public record PhaseDto
//{
//	public string PhaseName { get; init; } = string.Empty;
//}

public record GetProjectsResponse : ResponseBase
{
	public IEnumerable<ProjectDto> Items { get; init; } = [];
	public int TotalCount { get; init; }
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
}


