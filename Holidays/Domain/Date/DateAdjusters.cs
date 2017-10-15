using System;

namespace Holidays.Domain.Date
{
    public static class DateAdjusters
    {
        public static Func<LocalDate, LocalDate> FirstInMonth(DayOfWeek weekDay)
        {
            return date =>
            {
                var newDate = new LocalDate(date.Year, date.Month, 1);
                while (newDate.DayOfWeek != weekDay)
                {
                    newDate = newDate.PlusDays(1);
                }

                return newDate;
            };
        }

        public static Func<LocalDate, LocalDate> LastInMonth(DayOfWeek weekDay)
        {
            return date =>
            {
                var daysInMonth = date.DaysInMonth;
                var newDate = new LocalDate(date.Year, date.Month, daysInMonth);
                while (newDate.DayOfWeek != weekDay)
                {
                    newDate = newDate.PlusDays(-1);
                }

                return newDate;
            };
        }

        public static Func<LocalDate, LocalDate> DayOfWeekInMonth(int week, DayOfWeek weekDay)
        {
            return date =>
            {
                var firstOccurrence = FirstInMonth(weekDay)(date);
                return firstOccurrence.PlusWeeks(week - 1);
            };
        }

        public static Func<LocalDate, LocalDate> NextOrSame(DayOfWeek weekDay)
        {
            return date =>
            {
                while (date.DayOfWeek != weekDay)
                {
                    date = date.PlusDays(1);
                }

                return date;
            };
        }

        public static Func<LocalDate, LocalDate> Previous(DayOfWeek weekDay)
        {
            return date =>
            {
                date = date.PlusDays(-1);
                while (date.DayOfWeek != weekDay)
                {
                    date = date.PlusDays(-1);
                }

                return date;
            };
        }

        /// <summary>
        /// Obtains a <see cref="Func{LocalDate, LocalDate}"/> that finds the next 
        /// leap day after the input date.
        /// <para>
        /// The adjuster returns the next occurance of February 29 after the input date.
        /// </para>
        /// </summary>
        /// <returns>An adjuster that finds the next leap day</returns>
        public static Func<LocalDate, LocalDate> NextLeapDay()
        {
            return date =>
            {
                // Already a leap year, move forward either 4 or 8 years
                if (date.Month == 2 && date.Day == 29)
                {
                    return EnsureLeapDay(date.Year + 4);
                }

                // Handle if before February 29 in a leap year
                if (date.IsLeapYear() && date.Month <= 2)
                {
                    return new LocalDate(date.Year, 2, 29);
                }

                // Handle any other date
                return EnsureLeapDay((date.Year / 4) * 4 + 4);
            };
        }

        /// <summary>
        /// Obtains a <see cref="Func{LocalDate, LocalDate}"/> that finds the next 
        /// leap day on or after the input date.
        /// <para>
        /// If the input date is February 29, the input date is returned unaltered.
        /// Otherwise, the adjuster returns the next occurance of February 29 after the input date.
        /// </para>
        /// </summary>
        /// <returns>An adjuster that finds the next leap day</returns>
        public static Func<LocalDate, LocalDate> NextOrSameLeapDay()
        {
            return date =>
            {
                // Already a leap year, return it
                if (date.Month == 2 && date.Day == 29)
                {
                    return date;
                }

                // Handle if before February 29 in a leap year
                if (date.IsLeapYear() && date.Month <= 2)
                {
                    return new LocalDate(date.Year, 2, 29);
                }

                // Handle any other date
                return EnsureLeapDay((date.Year / 4) * 4 + 4);
            };
        }

        // Handle 2100, which is not a leap year
        private static LocalDate EnsureLeapDay(int possibleLeapYear)
        {
            if (DateTime.IsLeapYear(possibleLeapYear))
            {
                return new LocalDate(possibleLeapYear, 2, 29);
            }

            return new LocalDate(possibleLeapYear + 4, 2, 29);
        }
    }
}
