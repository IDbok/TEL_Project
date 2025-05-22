using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.BLL.DTOs;

public record ProjectDto
{
	public string Name { get; init; } = default!;
	public string Code { get; init; } = default!;
	public ClassifierKey ClassifierCode { get; init; } = default!;
	public string Responsible { get; init; } = default!; // ПМ => email ??  авторизация через ADO
	public string? Phase { get; init; } = default!; // Этап
	public string? Customer { get; init; } = default!; // Заказчик
	public DateTime DateInitiation { get; init; } = default!; // переход от PreSale к Execution
	public DateTime? DateCreated { get; init; } = default!; // дата создания проекта
	public List<BudgetLineDto> BudgetLines { get; init; } = default!; // JSON строка с бюджетами
}
