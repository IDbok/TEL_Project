using System.ComponentModel.DataAnnotations.Schema;
using TEL_ProjectBus.DAL.Entities.Reference;

namespace TEL_ProjectBus.DAL.Interfaces;

public interface IClassifierable
{
	public int? ClassifierId { get; set; }
	[NotMapped]public Classifier? Classifier { get; set; }
}
