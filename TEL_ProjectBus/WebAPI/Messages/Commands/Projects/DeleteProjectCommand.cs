namespace TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

public record DeleteProjectCommand
{
    public int ProjectId { get; init; }
}
