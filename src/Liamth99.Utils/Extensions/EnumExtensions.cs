namespace Liamth99.Utils.Extensions;

/// <summary>
/// Provides extension methods for working with enumeration (enum) types.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Tries to retrieve a single attribute of the specified type from the enum value.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute to retrieve.</typeparam>
    /// <param name="enumVal">The enum value from which the attribute is to be retrieved.</param>
    /// <param name="attribute">
    /// When this method returns, contains the attribute of the specified type if found;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// <c>true</c> if exactly one attribute of the specified type is found on the enum value; otherwise, <c>false</c>.
    /// </returns>
#if NET8_0_OR_GREATER
    public static bool TryGetSingleAttributeOfType<TAttribute>(this Enum enumVal, [NotNullWhen(true)] out TAttribute? attribute)
#else
    public static bool TryGetSingleAttributeOfType<TAttribute>(this Enum enumVal, out TAttribute? attribute)
#endif
        where TAttribute : Attribute
    {
        attribute = enumVal.GetSingleAttributeOfType<TAttribute>();

        return attribute is not null;
    }

    /// <summary>
    /// Retrieves a single attribute of the specified type from the enum value.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute to retrieve.</typeparam>
    /// <param name="enumVal">The enum value from which the attribute is to be retrieved.</param>
    /// <returns>The attribute of the specified type if found; otherwise, <c>null</c>.</returns>
    public static TAttribute? GetSingleAttributeOfType<TAttribute>(this Enum enumVal)
        where TAttribute : Attribute
    {
        return enumVal
              .GetType()
              .GetField(enumVal.ToString())?
              .GetCustomAttributes(typeof(TAttribute) ,false)
              .OfType<TAttribute>()
              .SingleOrDefault();
    }


#if NET8_0_OR_GREATER
    /// <summary>
    /// Retrieves the display name of an enum value, if a <see cref="DisplayAttribute"/> is defined; otherwise, returns the enum value's name as a string.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>The display name from the <see cref="DisplayAttribute"/> if available, otherwise the string representation of the enum value's name.</returns>
    public static string DisplayName(this Enum enumValue)
    {
        if (enumValue.TryGetSingleAttributeOfType<DisplayAttribute>(out var displayAttribute))
        {
            if (!string.IsNullOrWhiteSpace(displayAttribute.Name))
                return displayAttribute.Name;
        }

        return enumValue.ToString();
    }
#endif
}