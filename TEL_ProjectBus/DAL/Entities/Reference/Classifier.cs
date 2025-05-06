using TEL_ProjectBus.DAL.Entities.Common;
using TEL_ProjectBus.DAL.Entities.Projects;

namespace TEL_ProjectBus.DAL.Entities.Reference;

public class Classifier : AuditableEntity
{
	public int Id { get; set; }
	public string ClassifierCode { get; set; } = string.Empty;
	public ICollection<Project> Projects { get; set; } = [];
}
