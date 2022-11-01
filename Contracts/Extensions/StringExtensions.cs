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

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static int ParseToInt(this string value)
        {
            value = value.SafeLower().SafeTrim();

            _ = int.TryParse(value, out int result);

            return result;
        }

        public static string Between(this string value, string firstString, string lastString)
        {
            string finalString;
            int Pos1 = value.IndexOf(firstString) + firstString.Length;
            int Pos2 = value.IndexOf(lastString);
            finalString = value[Pos1..Pos2];
            return finalString;
        }

        public static string GetUntilOrEmpty(this string value, string stopAt)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                int charLocation = value.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return value[..charLocation];
                }
            }

            return string.Empty;
        }
    }
}
