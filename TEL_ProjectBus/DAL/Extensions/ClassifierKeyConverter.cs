using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TEL_ProjectBus.DAL.Entities.Common;

namespace TEL_ProjectBus.DAL.Extensions;

public sealed class ClassifierKeyConverter
	: ValueConverter<ClassifierKey, int>
{
	public ClassifierKeyConverter()
		: base(
			id => id.Value,          // при сохранении: ClassifierKey → long
			v => new ClassifierKey(v)) // при чтении: long → ClassifierKey
	{ }
}

