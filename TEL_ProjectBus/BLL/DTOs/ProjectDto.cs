using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.BLL.DTOs;

public record ProjectDto
{
	public int Id { get; init; }
	public string Name { get; init; } = default!;
	public string Code { get; init; } = default!;
	public string Responsible { get; init; } = default!; // ПМ => email ??  авторизация через ADO
	public string? Phase { get; init; } = default!; // Этап
	public DateTime DateInitiation { get; init; } = default!; // переход от PreSale к Execution
	public DateTime? DateCreated { get; init; } = default!; // дата создания проекта
	public DateTime? DateChanged { get; init; } = default!;
	public string ChangedByUserId { get; init; } = null!;

	public ClassifierDto Classifier { get; init; } = default!;
	public CustomerDto Customer { get; init; } = default!; // Заказчик
	public List<BudgetLineDto> BudgetLines { get; init; } = default!; // JSON строка с бюджетами
}
