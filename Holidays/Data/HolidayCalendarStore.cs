using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Holidays.Domain.Calendar;

namespace Holidays.Data
{
    public class HolidayCalendarStore
    {
        private static readonly ConcurrentDictionary<string, HolidayCalendar> calendars;

        public static readonly HolidayCalendarStore Instance = new HolidayCalendarStore();

        static HolidayCalendarStore()
        {
            var existingCalendars = new Dictionary<string, HolidayCalendar>
            {
                { HolidayCalendarIds.LON, GlobalHolidayCalendars.LON },
                { HolidayCalendarIds.PAR, GlobalHolidayCalendars.PAR },
                { HolidayCalendarIds.FRA, GlobalHolidayCalendars.FRA },
                { HolidayCalendarIds.ZUR, GlobalHolidayCalendars.ZUR },
                { HolidayCalendarIds.TGT, GlobalHolidayCalendars.TGT },
                { HolidayCalendarIds.NYC, GlobalHolidayCalendars.NYC }
            };
            calendars = new ConcurrentDictionary<string, HolidayCalendar>(existingCalendars);
        }

        private HolidayCalendarStore()
        {
        }

        public IEnumerable<HolidayCalendar> GetAll()
        {
            return calendars.Select(p => p.Value);
        }

        public HolidayCalendar GetCalendar(string id)
        {
            if (calendars.TryGetValue(id.ToUpperInvariant(), out HolidayCalendar calendar))
            {
                return calendar;
            }

            return null;
        }
    }
}
