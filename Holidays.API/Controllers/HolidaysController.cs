using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Holidays.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Output = Holidays.API.OutputModel;

namespace Holidays.API.Controllers
{
    [ApiController]
    [Route("api/holidays")]
    public class HolidaysController : ControllerBase
    {
        private readonly ILogger<HolidaysController> logger;
        private readonly HolidayService holidayService;

        public HolidaysController(ILogger<HolidaysController> logger, HolidayService holidayService)
        {
            this.logger = logger;
            this.holidayService = holidayService;
        }

        /// <summary>
        /// Get all holidays for the passed year ordered by date
        /// </summary>
        /// <param name="year">year</param>
        /// <returns> 
        /// - 200 with all holidays
        /// </returns>
        /// 
        /// GET: /api/holidays/{year}
        [HttpGet("{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Output.HolidayOutputModel>> GetHolidays([Range(0, int.MaxValue)] int year)
            => Ok(holidayService.GetAll(year));


        /// <summary>
        /// Get all holidays for the passed month ordered by date
        /// </summary>
        /// <param name="year">>0</param>
        /// <param name="month">1-12</param>
        /// <returns> 
        /// - 200 with all holidays
        /// </returns>
        /// 
        /// GET: /api/holidays/{year}/{month}
        [HttpGet("{year}/{month}/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Output.HolidayOutputModel>> GetHolidays([Range(0, int.MaxValue)] int year, [Range(1, 12)] int month)
            => Ok(holidayService.GetAll(year, month));

        /// <summary>
        /// Get next holiday after specific date
        /// </summary>
        /// <param name="date">yyyy-MM-dd</param>
        /// <returns> 
        /// - 200 with information about next holiday
        /// </returns>
        /// 
        /// GET: /api/holidays/{yyyy-MM-dd}/next
        [HttpGet("{date}/next")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Output.HolidayOutputModel?> GetNextHoliday(DateTime date)
            => Ok(holidayService.GetNextHoliday(date));

        /// <summary>
        /// Get next holiday after specific date
        /// </summary>
        /// <returns> 
        /// - 200 with information about next holiday
        /// </returns>
        /// 
        /// GET: /api/holidays/next
        [HttpGet("next")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Output.HolidayOutputModel?> GetNextHoliday()
            => GetNextHoliday(DateTime.Now);

        /// <summary>
        /// Get information for the passed date
        /// </summary>
        /// <param name="dayOfMonth">day of month</param>
        /// <param name="month">month</param>
        /// <param name="year">year</param>
        /// <returns> 
        /// - 200 with information about the passed date 
        /// - 204 when no information is found
        /// - 400 when input data is not valid
        /// </returns>
        /// 
        /// GET: /api/holidays/2020/1/20/information
        [HttpGet("{year}/{month}/{dayOfMonth}/information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Output.HolidayOutputModel?> GetHoliday(
            [Range(0, int.MaxValue)] int year
            , [Range(1, 12)] int month
            , [Range(1, 31)] int dayOfMonth
            )
        {
            var result = holidayService.GetHoliday(dayOfMonth, month, year);

            return result.HasErrors switch
            {
                true => BadRequest(result.Errors),
                false => Ok(result.Data)
            };
        }

        /// <summary>
        /// Get information for the passed date
        /// </summary>
        /// <param name="date">Date to check format (yyyy-MM-dd)</param>
        /// <returns> 
        /// - 200 with information about the passed date 
        /// - 204 when no information is found
        /// - 400 when input data is not valid
        /// </returns>
        /// 
        /// GET: /api/holidays/2020-01-21/information
        [HttpGet("{date}/information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Output.HolidayOutputModel?> GetHoliday(DateTime date)
        {
            return GetHoliday(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// Check if the passed date information is holiday
        /// </summary>
        /// <param name="dayOfMonth">day of month</param>
        /// <param name="month">month</param>
        /// <param name="year">year</param>
        /// <returns> True | False </returns>
        /// 
        /// GET: /api/holidays/6/5/2020/is-holiday
        [HttpGet("{year}/{month}/{dayOfMonth}/is-holiday")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> IsHoliday(
            [Range(0, int.MaxValue)] int year
            , [Range(0, int.MaxValue)] int month
            , [Range(0, int.MaxValue)] int dayOfMonth)
        {
            var result = holidayService.IsHoliday(dayOfMonth, month, year);

            return result.HasErrors switch
            {
                true => BadRequest(result.Errors),
                false => Ok(result.Data)
            };
        }

        /// <summary>
        /// Check if the passed date information is holiday
        /// </summary>
        /// <param name="date">yyyy-MM-dd</param>
        /// <returns> True | False </returns>
        /// 
        /// GET: /api/holidays/2020-06-22/is-holiday
        [HttpGet("{date}/is-holiday")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> IsHoliday(DateTime date)
        {
            return IsHoliday(date.Year, date.Month, date.Day);
        }
    }
}
