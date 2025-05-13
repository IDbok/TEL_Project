namespace TEL_ProjectBus.WebAPI.Messages.Queries;

//public record ProjectProfileDto{
//	public int ProjectId { get; init; }
//}

public record  GetProjectProfileResponse
{
	public int ProjectId { get; init; }
	public string Name { get; init; } = default!;
	public string Code { get; init; } = default!;
	public string Responsible { get; init; } = default!; // Ответственное лицо. В каком формате выводим ? Это ProjectOwner из параметров?
	public DateTime StartDate { get; init; }
	public DateTime EndDate { get; init; }
	public string Customer { get; init; } = default!; 
	public string ProjectType { get; init; } = default!; // ProjectStage из параметров?
	public string CurrentStage { get; init; } = default!;
	public string Progress { get; init; } = default!;
}
