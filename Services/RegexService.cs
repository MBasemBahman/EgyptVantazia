using Contracts.Extensions;
using System.Text.RegularExpressions;

namespace Services
{
    public static class RegexService
    {
        public static bool ValidateRegex(string input, string regex)
        {
            return input.IsExisting() && regex.IsExisting() && Regex.Match(input, regex, RegexOptions.IgnoreCase).Success;
        }

        #region UserName

        public static string GetUserName(string input)
        {
            return ValidateEmail(input) ? input : GetEgyptPhone(input);
        }

        #endregion

        #region Egypt Phone

        public static bool ValidateEgyptPhone(string input)
        {
            return ValidateRegex(input, "^1[0125][0-9]{8}$") ||
                   ValidateRegex(input, "^01[0125][0-9]{8}$") ||
                   ValidateRegex(input, "^201[0125][0-9]{8}$") ||
                   ValidateRegex(input, "^0201[0125][0-9]{8}$");
        }

        public static string GetEgyptPhone(string input)
        {
            if (ValidateEgyptPhone(input))
            {
                if (input.Length == 10 && input.StartsWith("1"))
                {
                    input = $"20{input}";
                }
                if (input.Length == 11 && input.StartsWith("0"))
                {
                    input = $"2{input}";
                }
                else if (input.Length == 13 && input.StartsWith("02"))
                {
                    input = input[1..];
                }
            }

            return input;
        }

        #endregion

        #region Email

        public static bool ValidateEmail(string input)
        {
            return ValidateRegex(input, @"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$");
        }

        #endregion
    }
}
