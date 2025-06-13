using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

public record CreateProjectResponse
{
	public int ProjectId { get; init; }
}
