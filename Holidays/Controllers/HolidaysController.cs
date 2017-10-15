using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holidays.Data;
using Microsoft.AspNetCore.Mvc;

namespace Holidays.Controllers
{
    [Route("api/calendars")]
    public class HolidaysController : Controller
    {
        [HttpGet]
        public IActionResult GetAllHolidayCalendars()
        {
            var calendars = HolidayCalendarStore.Instance.GetAll();
            return Ok(calendars);
        }

        [HttpGet("{id}")]
        public IActionResult GetHolidayCalendar(string id)
        {
            var calendar = HolidayCalendarStore.Instance.GetCalendar(id);
            if (calendar != null)
            {
                return Ok(calendar);
            }

            return NotFound();
        }
    }
}
