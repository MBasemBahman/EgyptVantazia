using System.Globalization;

namespace Entities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateTimeString(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
        }

        public static string ToLongDateString(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy", CultureInfo.InvariantCulture);
        }

        public static string ToLongDateTimeString(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy hh:mm tt", CultureInfo.InvariantCulture);
        }

        public static string ToArabicFormat(this DateTime value)
        {
            return value.ToString("dddd, dd MMMM yyyy", new CultureInfo("ar-EG"));
        }
    }
}
