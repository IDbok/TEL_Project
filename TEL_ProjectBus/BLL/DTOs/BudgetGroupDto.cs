namespace TEL_ProjectBus.BLL.DTOs;

public record BudgetGroupDto
{
	public int Id { get; init; }
	public string Name { get; init; } = string.Empty;
}
