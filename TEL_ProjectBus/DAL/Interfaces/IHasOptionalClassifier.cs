using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Interfaces;

public interface IHasOptionalClassifier
{
	ClassifierKey? ClassifierId { get; set; }
}
