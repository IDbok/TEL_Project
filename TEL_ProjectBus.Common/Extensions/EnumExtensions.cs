using System.ComponentModel;
using System.Reflection;

namespace TEL_ProjectBus.Common.Extensions;
public static class EnumExtensions
{
	public static string GetDescription(this Enum value)
	{
		if (value == null) return string.Empty;

		var type = value.GetType();
		var name = Enum.GetName(type, value);

		if (string.IsNullOrEmpty(name))
			return value.ToString(); // fallback для неизвестного значения

		var field = type.GetField(name);
		var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

		//var field = value.GetType().GetField(value.ToString());
		//var attribute = field.GetCustomAttribute<DescriptionAttribute>();

		return attribute == null ? value.ToString() : attribute.Description;
	}

	public static int ToInt(this Enum value)
	{
		if (value == null) throw new ArgumentNullException(nameof(value));
		return Convert.ToInt32(value);
	}

	public static TEnum? ToEnum<TEnum>(int value) where TEnum : struct, Enum
	{
		if (Enum.IsDefined(typeof(TEnum), value))
			return (TEnum)(object)value;

		return null;
	}

}
