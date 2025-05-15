namespace TEL_ProjectBus.WebAPI.Messages.Commands.Projects;

public record CreateProjectCommand
{
	public string Name { get; init; } = default!;
	public string Code { get; init; } = default!;
	public string ClassifierCode { get; init; } = default!;
	public string Responsible { get; init; } = default!; // ПМ
	public string? Phase { get; init; } = default!; // Этап
	public string? Customer { get; init; } = default!; // Заказчик
	public DateTime DateInitialization { get; init; } = default!; // переход от PreSale к Execution
	public DateTime? DateCreated { get; init; } = default!; // дата создания проекта
	public string BudgetLines { get; init; } = default!; // JSON строка с бюджетами

}
