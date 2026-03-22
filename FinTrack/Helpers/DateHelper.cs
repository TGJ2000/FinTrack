namespace FinTrack.Helpers
{
    public static class DateHelper
    {
        public static DateTime? ToDateTime(this DateOnly? date)
        {
            return date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : null;
        }

        public static DateTime ToDateTimeOrNow(this DateOnly? date)
        {
            return date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : DateTime.Now;
        }
    }
}