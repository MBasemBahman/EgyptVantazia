using System.Globalization;

namespace Contracts.Extensions
{
    public static class CronExpression
    {
        public static string EveryMinutes(int minute)
        {
            return $"*/{minute} * * * *";
        }

        public static string EveryDayOfMonth(int day, int hour, int minute)
        {
            return $"{minute} {hour} {day} * *";
        }

        public static string ToString(DateTime value)
        {
            return value.ToString("mm HH dd MM") + " *";
        }

        public static string ToString(DateTime startDateTime, DateTime EndDateTime, int Minute)
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
