namespace Contracts.Extensions
{
    public static class StringExtensions
    {
        public static string SafeTrim(this string value)
        {
            return value == null ? string.Empty : value.Trim();
        }

        public static string SafeReplace(this string value, string oldChar, string newChar)
        {
            return value == null ? string.Empty : value.Replace(oldChar, newChar);
        }

        public static string SafeLower(this string value)
        {
            return value == null ? string.Empty : value.ToLowerInvariant();
        }

        public static bool IsExisting(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static int ParseToInt(this string value)
        {
            value = value.SafeLower().SafeTrim();

            _ = int.TryParse(value, out int result);

            return result;
        }
    }
}
