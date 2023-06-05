using System.Globalization;

namespace Contracts.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateTimeString(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy hh:mm tt");
        }

        public static DateTime ToEgypt(this DateTime value)
        {
            return value.AddHours(3);
        }

        public static string ToLongDateString(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy");
        }

        public static string ToLongDateTimeString(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy hh:mm tt");
        }

        public static string ToArabicFormat(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy", new CultureInfo("ar-EG"));
        }

        public static string ToCronExpression(this DateTime value)
        {
            return value.ToString("mm HH dd MM") + " *";
        }

        public static string ToCronExpression(this DateTime startDateTime, DateTime EndDateTime, int Minute)
        {
            List<string> mins = new();
            List<string> hours = new();
            List<string> days = new();
            List<string> months = new();

            for (DateTime dateTime = startDateTime; dateTime < EndDateTime; dateTime = dateTime.AddMinutes(Minute))
            {
                string min = $"{dateTime.ToString("mm", CultureInfo.InvariantCulture)}";
                if (!mins.Contains(min))
                {
                    mins.Add(min);
                }

                string hour = $"{dateTime.ToString("HH", CultureInfo.InvariantCulture)}";
                if (!hours.Contains(hour))
                {
                    hours.Add(hour);
                }

                string day = $"{dateTime.ToString("dd", CultureInfo.InvariantCulture)}";
                if (!days.Contains(day))
                {
                    days.Add(day);
                }

                string month = $"{dateTime.ToString("MM", CultureInfo.InvariantCulture)}";
                if (!months.Contains(month))
                {
                    months.Add(month);
                }
            }

            string cronExpression = "";

            mins.ForEach(min => cronExpression += $"{min},");
            cronExpression = cronExpression.Remove(cronExpression.Length - 1, 1) + " ";

            hours.ForEach(hour => cronExpression += $"{hour},");
            cronExpression = cronExpression.Remove(cronExpression.Length - 1, 1) + " ";

            days.ForEach(day => cronExpression += $"{day},");
            cronExpression = cronExpression.Remove(cronExpression.Length - 1, 1) + " ";

            months.ForEach(month => cronExpression += $"{month},");
            cronExpression = cronExpression.Remove(cronExpression.Length - 1, 1) + " ";

            cronExpression = cronExpression.Remove(cronExpression.Length - 1, 1) + " ";

            cronExpression += $"*";

            return cronExpression;
        }
    }
}
