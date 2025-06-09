using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.BLL.DTOs;

public record ClassifierDto
{	public ClassifierKey Id { get; init; }
	public string ClassifierCode { get; init; } = default!;
}
