using System;
using System.Collections.Generic;
using NodaTime.Extensions;

namespace Holidays.Domain.Date
{
    public struct LocalDate : IComparable<LocalDate>
    {
        private readonly NodaTime.LocalDate localDate;

        public LocalDate(int year, int month, int day)
        {
            this.localDate = new NodaTime.LocalDate(year, month, day);
        }

        private LocalDate(NodaTime.LocalDate localDate)
        {
            this.localDate = localDate;
        }

        public int Day => localDate.Day;

        public int Month => localDate.Month;

        public int Year => localDate.Year;

        public int DaysInMonth => localDate.Calendar.GetDaysInMonth(localDate.Year, localDate.Month);

        public int DaysSinceEpoch => Convert.ToInt32(localDate.ToDateTimeUnspecified().ToOADate() - new DateTime(1970, 1, 1).ToOADate());

        public DayOfWeek DayOfWeek => IsoDayOfWeekExtensions.ToDayOfWeek(localDate.DayOfWeek);

        public static bool operator ==(LocalDate date1, LocalDate date2)
        {
            return date1.localDate == date2.localDate;
        }

        public static bool operator !=(LocalDate date1, LocalDate date2)
        {
            return date1.localDate != date2.localDate;
        }

        public static bool operator <(LocalDate date1, LocalDate date2)
        {
            return date1.localDate < date2.localDate;
        }

        public static bool operator <=(LocalDate date1, LocalDate date2)
        {
            return date1.localDate <= date2.localDate;
        }

        public static bool operator >(LocalDate date1, LocalDate date2)
        {
            return date1.localDate > date2.localDate;
        }

        public static bool operator >=(LocalDate date1, LocalDate date2)
        {
            return date1.localDate >= date2.localDate;
        }

        public bool IsLeapYear()
        {
            return DateTime.IsLeapYear(localDate.Year);
        }

        public LocalDate WithDayOfMonth(int dayOfMonth)
        {
            return new LocalDate(localDate.Year, localDate.Month, dayOfMonth);
        }

        public LocalDate With(Func<LocalDate, LocalDate> shift)
        {
            return shift(this);
        }

        public LocalDate PlusDays(int days)
        {
            return new LocalDate(localDate.PlusDays(days));
        }

        public LocalDate PlusWeeks(int weeks)
        {
            return new LocalDate(localDate.PlusWeeks(weeks));
        }

        public LocalDate PlusMonths(int months)
        {
            return new LocalDate(localDate.PlusMonths(months));
        }

        public static int DaysBetween(LocalDate firstDate, LocalDate secondDate)
        {
            return secondDate.DaysSinceEpoch - firstDate.DaysSinceEpoch;
        }

        public static IEnumerable<LocalDate> GenerateDates(LocalDate startDate, LocalDate endDate, bool endInclusive)
        {
            var list = new List<LocalDate>();
            var date = startDate;
            while (date < endDate)
            {
                list.Add(date);
                date = date.PlusDays(1);
            }

            if (endInclusive)
            {
                list.Add(date);
            }

            return list;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (LocalDate)obj;
            return localDate.Equals(other.localDate);
        }

        public override int GetHashCode()
        {
            return localDate.GetHashCode();
        }

        public override string ToString()
        {
            return localDate.ToString("dd-MMM-yyyy", null);
        }

        public int CompareTo(LocalDate other)
        {
            return this.localDate.CompareTo(other.localDate);
        }
    }
}
