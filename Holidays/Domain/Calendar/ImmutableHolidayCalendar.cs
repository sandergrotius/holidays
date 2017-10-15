/*
 This code is based on an implementation in OpenGamma's Strata (https://github.com/OpenGamma/Strata)
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Holidays.Domain.Date;

namespace Holidays.Domain.Calendar
{
    public class ImmutableHolidayCalendar : HolidayCalendar
    {
        private readonly string id;
        private readonly string description;
        private readonly IReadOnlyCollection<LocalDate> holidays;
        private readonly IReadOnlyCollection<DayOfWeek> weekendDays;

        public ImmutableHolidayCalendar(
            string id,
            string description,
            ICollection<LocalDate> holidays,
            DayOfWeek firstWeekendDay,
            DayOfWeek secondWeekendDay)
            : this(id, description, holidays, (ICollection<DayOfWeek>) new List<DayOfWeek> { firstWeekendDay, secondWeekendDay })
        {
        }

        public ImmutableHolidayCalendar(
            string id, 
            string description,
            ICollection<LocalDate> holidays, 
            ICollection<DayOfWeek> weekendDays)
        {
            ArgumentChecker.NotNull(id, nameof(id));
            ArgumentChecker.NotNull(description, nameof(description));
            ArgumentChecker.NotNull(holidays, nameof(holidays));
            ArgumentChecker.NotNull(weekendDays, nameof(weekendDays));
            this.id = id;
            this.description = description;
            this.holidays = new ReadOnlyCollection<LocalDate>(holidays.OrderBy(p => p).ToList());
            this.weekendDays = new ReadOnlyCollection<DayOfWeek>(weekendDays.ToList());
        }

        public static ImmutableHolidayCalendar Combined(ImmutableHolidayCalendar calendar1, ImmutableHolidayCalendar calendar2)
        {
            if (calendar1 == calendar2)
            {
                return ArgumentChecker.NotNull(calendar1, nameof(calendar1));
            }

            var newHolidays = new ReadOnlyCollection<LocalDate>(Enumerable.Concat<LocalDate>(calendar1.holidays, calendar2.holidays).ToList());
            var newWeekends = new ReadOnlyCollection<DayOfWeek>(Enumerable.Concat<DayOfWeek>(calendar1.weekendDays, calendar2.weekendDays).ToList());
            var description = calendar1.description + " combined with " + calendar2.description;
            return new ImmutableHolidayCalendar(calendar1.id + calendar2.id, description, newHolidays, newWeekends);
        }

        public override string Id => id;

        public override string Description => description;

        public override bool IsHoliday(LocalDate date)
        {
            if (Enumerable.Contains(weekendDays, date.DayOfWeek) || Enumerable.Contains(holidays, date))
            {
                return true;
            }

            return false;
        }
    }
}
