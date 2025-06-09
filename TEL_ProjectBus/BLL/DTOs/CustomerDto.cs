namespace TEL_ProjectBus.BLL.DTOs;

public record CustomerDto
{
	public int Id { get; init; }
	public string Name { get; init; } = default!;
	public string? Address { get; init; }
	public string CompanyName { get; init; } = default!;
	public string ContactPerson { get; init; } = default!;
	public string? Uuid { get; init; } = null;

}
