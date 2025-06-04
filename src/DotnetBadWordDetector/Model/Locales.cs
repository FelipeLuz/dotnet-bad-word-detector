using System.ComponentModel;
using System.Reflection;

namespace DotnetBadWordDetector.Model;

public enum Locales
{
	ENGLISH,
	SPANISH,
	PORTUGUESE,
	RUSSIAN_CYRILLIC
}

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString().ToLowerInvariant() : attribute.Description.ToLowerInvariant();
    }
}