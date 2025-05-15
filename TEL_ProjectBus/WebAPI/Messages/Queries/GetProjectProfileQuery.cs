using MassTransit.Mediator;

namespace TEL_ProjectBus.WebAPI.Messages.Queries;

public record GetProjectProfileQuery
{
	public int ProjectId { get; init; }

}
