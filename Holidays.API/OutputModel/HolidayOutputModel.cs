using Holidays.API.Common;
using Holidays.API.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Holidays.API.OutputModel
{
    public class HolidayOutputModel
    {
        public HolidayOutputModel(FloatingHoliday floatingHoliday) 
            => (Name, Date) = (floatingHoliday.Name, floatingHoliday.Date);

        public HolidayOutputModel(ConstantHoliday constantHoliday, int year) 
            => (Name, Date) = (constantHoliday.Name, new DateTime(year, constantHoliday.Month, constantHoliday.DayOfMonth));

        public HolidayOutputModel(string name, DateTime date) 
            => (Name, Date) = (name, date);

        public string Name { get; set; }

        /// <summary>
        /// Date of the Holdiay (format: yyyy-MM-dd)
        /// </summary>
        [DataType(DataType.Date)]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Date { get; set; }
    }
}
