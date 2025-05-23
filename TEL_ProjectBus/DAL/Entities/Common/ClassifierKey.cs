namespace TEL_ProjectBus.DAL.Entities.Common;

/// <summary>
/// Сильный ID классификатора. Внешне выглядит как long,
/// но компилятор уже не спутать с любым другим long.
/// </summary>
public readonly record struct ClassifierKey(int Value)
{
	public static implicit operator int(ClassifierKey id) => id.Value;
	public static explicit operator ClassifierKey(int value) => new(value);
}
