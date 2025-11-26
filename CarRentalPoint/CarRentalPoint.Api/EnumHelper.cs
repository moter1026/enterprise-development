namespace CarRentalPoint.Api;

public static class EnumHelper
{
    /// <summary>
    /// Tries to parse a string into a specified enum type TEnum.
    /// Returns true if parsing succeeded, false otherwise, with an error message.
    /// </summary>
    public static bool TryParseEnum<TEnum>(
        string value,
        out TEnum result,
        out string error) where TEnum : struct, Enum
    {
        error = "";

        if (!Enum.TryParse(value, true, out result))
        {
            error = $"Invalid value: {value}. Allowed: {string.Join(", ", Enum.GetNames(typeof(TEnum)))}";
            return false;
        }

        return true;
    }
}