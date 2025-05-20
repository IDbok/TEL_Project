using TEL_ProjectBus.BLL.DTOs;
using TEL_ProjectBus.WebAPI.Common;

namespace TEL_ProjectBus.WebAPI.Messages.Queries.Projects;

public record  GetProjectProfileResponse : ResponseBase
{
	public ProjectProfileDto? ProjectProfile { get; init; }
}
