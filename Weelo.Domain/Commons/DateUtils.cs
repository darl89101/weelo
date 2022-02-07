namespace Weelo.Domain.Commons
{
    public class DateUtils
    {
        public static DateTime GetCurrentDate()
            => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
    }
}
