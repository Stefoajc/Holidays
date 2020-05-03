using System;

namespace Holidays.API.Models
{

    /// <summary>
    /// Like Easter
    /// Palm day
    /// Whit day
    /// Good Friday
    /// Ascension Day
    /// </summary>
    public class FloatingHoliday
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
