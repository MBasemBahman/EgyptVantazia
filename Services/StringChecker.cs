using System.Text.RegularExpressions;

namespace Services
{
    public class StringChecker
    {
        public static bool HasArabicCharacters(string text)
        {
            Regex regex = new("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");

            return regex.IsMatch(text);
        }
    }
}
