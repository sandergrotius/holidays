/*
 This code is based on an implementation in OpenGamma's Strata (https://github.com/OpenGamma/Strata)
 */

using Holidays.Domain.Date;

namespace Holidays.Domain.Calendar
{
    public class CombinedHolidayCalendar : HolidayCalendar
    {
        private readonly HolidayCalendar calendar1;
        private readonly HolidayCalendar calendar2;

        public CombinedHolidayCalendar(HolidayCalendar calendar1, HolidayCalendar calendar2)
        {
            ArgumentChecker.NotNull(calendar1, nameof(calendar1));
            ArgumentChecker.NotNull(calendar2, nameof(calendar2));
            this.calendar1 = calendar1;
            this.calendar2 = calendar2;
        }

        public override bool IsHoliday(LocalDate date)
        {
            return calendar1.IsHoliday(date) || calendar2.IsHoliday(date);
        }

        public override string Id => calendar1.Id + calendar2.Id;

        public override string Description => calendar1.Description + " combined with " + calendar2.Description;
    }
}
