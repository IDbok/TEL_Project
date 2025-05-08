using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Projects;

public class ProjectPortfolioCross : AuditableEntity, IHasIdentity<int>
{
	public int Id { get; set; }
	public int ProjectId { get; set; }
	public int PortfolioId { get; set; }

	public Project Project { get; set; } = null!;
	public ProjectPortfolio Portfolio { get; set; } = null!;
}
