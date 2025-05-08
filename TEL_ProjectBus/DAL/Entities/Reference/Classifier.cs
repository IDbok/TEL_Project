using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;
using TEL_ProjectBus.DAL.Interfaces;

namespace TEL_ProjectBus.DAL.Entities.Reference;

public class Classifier : AuditableEntity, IHasIdentity<long>
{
	public long Id { get; set; }
	public string ClassifierCode { get; set; } = string.Empty;
	public ICollection<Project> Projects { get; set; } = [];
}
