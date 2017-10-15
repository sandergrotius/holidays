/*
 This code is based on an implementation in OpenGamma's Strata (https://github.com/OpenGamma/Strata)
*/

using System;
using System.Linq;
using Holidays.Domain.Date;

namespace Holidays.Domain.Calendar
{
    public abstract class HolidayCalendar
    {
        public abstract bool IsHoliday(LocalDate date);

        public virtual bool IsBusinessDay(LocalDate date)
        {
            return !IsHoliday(date);
        }

        public Func<LocalDate, LocalDate> AdjustBy(int amount)
        {
            return date => Shift(date, amount);
        }

        public virtual LocalDate Shift(LocalDate date, int amount)
        {
            var adjusted = date;
            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    adjusted = Next(adjusted);
                }
            }
            else if (amount < 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    adjusted = Previous(adjusted);
                }
            }

            return adjusted;
        }

        public virtual LocalDate Next(LocalDate date)
        {
            var next = date.PlusDays(1);
            return IsHoliday(next) ? Next(next) : next;
        }

        public virtual LocalDate NextOrSame(LocalDate date)
        {
            return IsHoliday(date) ? Next(date) : date;
        }

        public virtual LocalDate Previous(LocalDate date)
        {
            var previous = date.PlusDays(-1);
            return IsHoliday(previous) ? Previous(previous) : previous;
        }

        public virtual LocalDate PreviousOrSame(LocalDate date)
        {
            return IsHoliday(date) ? Previous(date) : date;
        }

        public virtual LocalDate NextSameOrLastInMonth(LocalDate date)
        {
            var nextOrSame = NextOrSame(date);
            return nextOrSame.Month != date.Month ? Previous(date) : nextOrSame;
        }

        public bool IsLastBusinessDayOfMonth(LocalDate date)
        {
            return IsBusinessDay(date) && Next(date).Month != date.Month;
        }

        public LocalDate LastBusinessDayOfMonth(LocalDate date)
        {
            return PreviousOrSame(date.WithDayOfMonth(date.DaysInMonth));
        }

        public virtual int DaysBetween(LocalDate startInclusive, LocalDate endExclusive)
        {
            return LocalDate.GenerateDates(startInclusive, endExclusive, false).Count(IsBusinessDay);
        }

        public virtual HolidayCalendar CombinedWith(HolidayCalendar other)
        {
            if (this.Equals(other))
            {
                return this;
            }

            return new CombinedHolidayCalendar(this, other);
        }

        public abstract string Id { get; }

        public abstract string Description { get; }
    }
}
