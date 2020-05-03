namespace Holidays.API.Models
{
    /// <summary>
    /// Name days, National holidays, etc.
    /// </summary>
    public class ConstantHoliday
    {
        public int DayOfMonth { get; set; }
        public int Month { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
