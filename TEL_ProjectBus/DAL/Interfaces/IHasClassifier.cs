using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Interfaces;

public interface IHasClassifier
{
	ClassifierKey ClassifierId { get; set; }
}
