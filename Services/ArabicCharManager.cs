namespace Services
{
    public class ArabicCharService
    {
        private static Dictionary<char, char> GetDataSet()
        {
            return new Dictionary<char, char>
            {
                // ا
                { 'ء', 'ا' },
                { 'آ', 'ا' },
                { 'أ', 'ا' },
                { 'ؤ', 'ا' },
                { 'إ', 'ا' },
                { 'ئ', 'ا' },

                // ى
                { 'ي', 'ى' },

                // ة
                { 'ة', 'ه' }
            }; ;
        }

        public static string GetBaseString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            Dictionary<char, char> dataSet = GetDataSet();

            string finalText = "";

            for (int i = 0; i < text.Length; i++)
            {
                if (dataSet.ContainsKey(text[i]))
                {
                    finalText += dataSet[text[i]];
                }
                else
                {
                    finalText += text[i];
                }
            }

            return finalText;
        }
    }
}
