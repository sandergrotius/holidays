/*
 These holiday calendar date calculations are based on the implementation
 in OpenGamma's Strata (https://github.com/OpenGamma/Strata), which in
 turn is based on publicly available data.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Holidays.Domain.Date;

namespace Holidays.Domain.Calendar
{
    /// <summary>
    /// Implementation of some common global holiday calendars
    /// </summary>
    public static class GlobalHolidayCalendars
    {
        private static readonly HolidayCalendar lon;
        private static readonly HolidayCalendar par;
        private static readonly HolidayCalendar fra;
        private static readonly HolidayCalendar zur;
        private static readonly HolidayCalendar target;
        private static readonly HolidayCalendar nyc;

        /// <summary>
        /// The holiday calendar for London, United Kingdom, with code 'LON'.
        /// </summary>
        public static HolidayCalendar LON => lon;

        /// <summary>
        /// The holiday calendar for Paris, France, with code 'PAR'.
        /// </summary>
        public static HolidayCalendar PAR => par;

        /// <summary>
        /// The holiday calendar for Frankfurt, Germany, with code 'FRA'.
        /// </summary>
        public static HolidayCalendar FRA => fra;

        /// <summary>
        /// The holiday calendar for Zurich, Switzerland, with code 'ZUR'.
        /// </summary>
        public static HolidayCalendar ZUR => zur;

        /// <summary>
        /// The holiday calendar for the European Union TARGET system, with code 'TARGET'.
        /// </summary>
        public static HolidayCalendar TGT => target;

        /// <summary>
        /// The holiday calendar for New York, Unites States, with code 'NYC'.
        /// </summary>
        public static HolidayCalendar NYC => nyc;
        static GlobalHolidayCalendars()
        {
            lon = GenerateLondon();
            par = GenerateParis();
            fra = GenerateFrankfurt();
            zur = GenerateZurich();
            target = GenerateEuropeanTarget();
            nyc = GenerateUsNewYork();
        }

        private static ImmutableHolidayCalendar GenerateLondon()
        {
            var holidays = new List<LocalDate>(2000);
            for (int year = 1950; year < 2099; year++)
            {
                // New Year
                if (year >= 1974)
                {
                    holidays.Add(BumpToMonday(First(year, 1)));
                }

                // Easter
                holidays.Add(Easter(year).PlusDays(-2));
                holidays.Add(Easter(year).PlusDays(1));

                // Early May
                if (year == 1995)
                {
                    holidays.Add(new LocalDate(1995, 5, 8));
                }
                else if (year >= 1978)
                {
                    holidays.Add(First(year, 5).With(DateAdjusters.FirstInMonth(DayOfWeek.Monday)));
                }

                // Spring
                if (year == 2002)
                {
                    // Golden Jubilee
                    holidays.Add(new LocalDate(2002, 6, 3));
                    holidays.Add(new LocalDate(2002, 6, 4));
                }
                else if (year == 2012)
                {
                    // Diamond Jubilee
                    holidays.Add(new LocalDate(2012, 6, 4));
                    holidays.Add(new LocalDate(2012, 6, 5));
                }
                else if (year == 1967 || year == 1970)
                {
                    holidays.Add(First(year, 5).With(DateAdjusters.LastInMonth(DayOfWeek.Monday)));
                }
                else if (year < 1971)
                {
                    // Whitsun
                    holidays.Add(Easter(year).PlusDays(50));
                }
                else
                {
                    holidays.Add(First(year, 5).With(DateAdjusters.LastInMonth(DayOfWeek.Monday)));
                }

                // Summer
                if (year < 1965)
                {
                    holidays.Add(First(year, 8).With(DateAdjusters.FirstInMonth(DayOfWeek.Monday)));
                }
                else if (year < 1971)
                {
                    holidays.Add(First(year, 8).With(DateAdjusters.LastInMonth(DayOfWeek.Saturday)).PlusDays(2));
                }
                else
                {
                    holidays.Add(First(year, 8).With(DateAdjusters.LastInMonth(DayOfWeek.Monday)));
                }

                // Christmas
                holidays.Add(ChristmasBumpedSaturdaySunday(year));
                holidays.Add(BoxingDayBumpedSaturdaySunday(year));
            }

            holidays.Add(new LocalDate(2011, 4, 29)); // Royal wedding
            holidays.Add(new LocalDate(1999, 12, 31)); // Millenium
            RemoveSatSun(holidays);
            return new ImmutableHolidayCalendar(
                HolidayCalendarIds.LON,
                "The holiday calendar for London, United Kingdom, with code 'LON'",
                holidays, 
                DayOfWeek.Saturday,
                DayOfWeek.Sunday);
        }

        private static ImmutableHolidayCalendar GenerateParis()
        {
            var holidays = new List<LocalDate>();
            for (int year = 1950; year < 2099; year++)
            {
                holidays.Add(new LocalDate(year, 1, 1));
                holidays.Add(Easter(year).PlusDays(-2));
                holidays.Add(Easter(year).PlusDays(1));
                holidays.Add(new LocalDate(year, 5, 1));
                holidays.Add(new LocalDate(year, 5, 8));
                holidays.Add(Easter(year).PlusDays(39));

                if (year <= 2004 || year >= 2008)
                {
                    holidays.Add(Easter(year).PlusDays(50));
                }

                holidays.Add(new LocalDate(year, 7, 14));
                holidays.Add(new LocalDate(year, 8, 15));
                holidays.Add(new LocalDate(year, 11, 1));
                holidays.Add(new LocalDate(year, 11, 11));
                holidays.Add(new LocalDate(year, 12, 25));
                holidays.Add(new LocalDate(year, 12, 26));
            }

            holidays.Add(new LocalDate(1999, 12, 31));
            ApplyBridging(holidays);
            RemoveSatSun(holidays);
            return new ImmutableHolidayCalendar(
                HolidayCalendarIds.PAR,
                "The holiday calendar for Paris, France, with code 'PAR'",
                holidays, 
                DayOfWeek.Saturday, 
                DayOfWeek.Sunday);
        }

        private static ImmutableHolidayCalendar GenerateFrankfurt()
        {
            var holidays = new List<LocalDate>(2000);
            for (int year = 1950; year <= 2099; year++)
            {
                // New Year's Day
                holidays.Add(new LocalDate(year, 1, 1));
                // Good Friday
                holidays.Add(Easter(year).PlusDays(-2));
                // Easter Monday
                holidays.Add(Easter(year).PlusDays(1));
                // Labour Day
                holidays.Add(new LocalDate(year, 5, 1));
                // Ascension Day
                holidays.Add(Easter(year).PlusDays(39));
                // Whit Monday
                holidays.Add(Easter(year).PlusDays(50));
                // Corpus Christi
                holidays.Add(Easter(year).PlusDays(60));
                // German Unity
                if (year >= 2000)
                {
                    holidays.Add(new LocalDate(year, 10, 3));
                }

                // Repentance
                if (year <= 1994)
                {
                    // Wednesday before the Sunday that is 2 weeks before first advent, which is 4th Sunday before Christmas
                    holidays.Add(new LocalDate(year, 12, 25)
                        .With(DateAdjusters.Previous(DayOfWeek.Sunday))
                        .PlusWeeks(-6)
                        .PlusDays(-4));
                }

                // Christmas Day
                holidays.Add(new LocalDate(year, 12, 25));
                // St Stephen
                holidays.Add(new LocalDate(year, 12, 26));
                // New Year's Eve
                holidays.Add(new LocalDate(year, 12, 31));
            }

            // Reformation Day
            holidays.Add(new LocalDate(2017, 10, 31));
            RemoveSatSun(holidays);
            return new ImmutableHolidayCalendar(
                HolidayCalendarIds.FRA,
                "The holiday calendar for Frankfurt, Germany, with code 'FRA'",
                holidays, 
                DayOfWeek.Saturday, 
                DayOfWeek.Sunday);
        }

        private static ImmutableHolidayCalendar GenerateZurich()
        {
            var holidays = new List<LocalDate>();
            for (int year = 1950; year < 2099; year++)
            {
                holidays.Add(new LocalDate(year, 1, 1));
                holidays.Add(new LocalDate(year, 1, 2));
                holidays.Add(Easter(year).PlusDays(-2));
                holidays.Add(Easter(year).PlusDays(1));
                holidays.Add(new LocalDate(year, 5, 1));
                holidays.Add(Easter(year).PlusDays(39));
                holidays.Add(Easter(year).PlusDays(50));
                holidays.Add(new LocalDate(year, 8, 1));
                holidays.Add(new LocalDate(year, 12, 25));
                holidays.Add(new LocalDate(year, 12, 26));
            }

            holidays.Add(new LocalDate(1999, 12, 31));
            holidays.Add(new LocalDate(2000, 1, 3));
            RemoveSatSun(holidays);
            return new ImmutableHolidayCalendar(
                HolidayCalendarIds.ZUR,
                "The holiday calendar for Zurich, Switzerland, with code 'ZUR'", 
                holidays, 
                DayOfWeek.Saturday, 
                DayOfWeek.Sunday);
        }

        private static ImmutableHolidayCalendar GenerateEuropeanTarget()
        {
            var holidays = new List<LocalDate>();
            for (int year = 1997; year < 2099; year++)
            {
                if (year >= 2000)
                {
                    holidays.Add(new LocalDate(year, 1, 1));
                    holidays.Add(Easter(year).PlusDays(-2));
                    holidays.Add(Easter(year).PlusDays(1));
                    holidays.Add(new LocalDate(year, 5, 1));
                    holidays.Add(new LocalDate(year, 12, 25));
                    holidays.Add(new LocalDate(year, 12, 26));
                }
                else
                {
                    holidays.Add(new LocalDate(year, 1, 1));
                    holidays.Add(new LocalDate(year, 12, 25));
                }

                if (year == 1999 || year == 2001)
                {
                    holidays.Add(new LocalDate(year, 12, 31));
                }
            }
                
            RemoveSatSun(holidays);
            return new ImmutableHolidayCalendar(
                HolidayCalendarIds.TGT,
                "The holiday calendar for the European Union TARGET system, with code 'TARGET'", 
                holidays, 
                DayOfWeek.Saturday, 
                DayOfWeek.Sunday);
        }

        private static ImmutableHolidayCalendar GenerateUsNewYork()
        {
            var holidays = new List<LocalDate>(2000);
            for (int year = 1950; year < 2099; year++)
            {
                UsCommon(holidays, year, false, true, 1986);
            }

            RemoveSatSun(holidays);
            return new ImmutableHolidayCalendar(
                HolidayCalendarIds.NYC,
                "The holiday calendar for New York, United States, with code 'NYC'",
                holidays, 
                DayOfWeek.Saturday, 
                DayOfWeek.Sunday);
        }

        private static void UsCommon(List<LocalDate> holidays, int year, bool bumpBack, bool columbusVeteran, int mlkStartYear)
        {
            holidays.Add(BumpSundayToMonday(new LocalDate(year, 1, 1))); // New Year's Day
            if (year >= mlkStartYear) // Martin Luther King day
            {
                holidays.Add(new LocalDate(year, 1, 1).With(DateAdjusters.DayOfWeekInMonth(3, DayOfWeek.Monday)));
            }

            // Washington
            if (year < 1971)
            {
                holidays.Add(BumpSundayToMonday(new LocalDate(year, 2, 22)));
            }
            else
            {
                holidays.Add(new LocalDate(year, 2, 1).With(DateAdjusters.DayOfWeekInMonth(3, DayOfWeek.Monday)));
            }

            // Memorial Day
            if (year < 1971)
            {
                holidays.Add(BumpSundayToMonday(new LocalDate(year, 5, 30)));
            }
            else
            {
                holidays.Add(new LocalDate(year, 2, 1).With(DateAdjusters.LastInMonth(DayOfWeek.Monday)));
            }

            // Labor Day
            holidays.Add(new LocalDate(year, 9, 1).With(DateAdjusters.FirstInMonth(DayOfWeek.Monday)));

            // Columbus Day
            if (columbusVeteran)
            {
                if (year < 1971)
                {
                    holidays.Add(BumpSundayToMonday(new LocalDate(year, 10, 12)));
                }
                else
                {
                    holidays.Add(new LocalDate(year, 10, 1).With(DateAdjusters.DayOfWeekInMonth(2, DayOfWeek.Monday)));
                }
            }

            // Veteran's Day
            if (columbusVeteran)
            {
                if (year >= 1971 && year < 1978)
                {
                    holidays.Add(
                        new LocalDate(year, 10, 1).With(DateAdjusters.DayOfWeekInMonth(4, DayOfWeek.Monday)));
                }
                else
                {
                    holidays.Add(BumpSundayToMonday(new LocalDate(year, 11, 11)));
                }
            }

            // Thanksgiving
            holidays.Add(new LocalDate(year, 11, 1).With(DateAdjusters.DayOfWeekInMonth(4, DayOfWeek.Thursday)));

            // Independence Day & Christmas Day
            if (bumpBack)
            {
                holidays.Add(BumpToFridayOrMonday(new LocalDate(year, 7, 4)));
                holidays.Add(BumpToFridayOrMonday(new LocalDate(year, 12, 25)));
            }
            else
            {
                holidays.Add(BumpSundayToMonday(new LocalDate(year, 7, 4)));
                holidays.Add(BumpSundayToMonday(new LocalDate(year, 12, 25)));
            }
        }

        private static LocalDate BumpToMonday(LocalDate date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                return date.PlusDays(2);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.PlusDays(1);
            }

            return date;
        }

        private static LocalDate BumpSundayToMonday(LocalDate date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.PlusDays(1);
            }

            return date;
        }

        private static LocalDate BumpToFridayOrMonday(LocalDate date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                return date.PlusDays(-1);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.PlusDays(1);
            }

            return date;
        }

        private static LocalDate ChristmasBumpedSaturdaySunday(int year)
        {
            var baseDate = new LocalDate(year, 12, 25);
            if (baseDate.DayOfWeek == DayOfWeek.Saturday || baseDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return new LocalDate(year, 12, 27);
            }

            return baseDate;
        }

        private static LocalDate BoxingDayBumpedSaturdaySunday(int year)
        {
            var baseDate = new LocalDate(year, 12, 26);
            if (baseDate.DayOfWeek == DayOfWeek.Saturday || baseDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return new LocalDate(year, 12, 28);
            }

            return baseDate;
        }

        private static LocalDate First(int year, int month)
        {
            return new LocalDate(year, month, 1);
        }

        private static void RemoveSatSun(List<LocalDate> holidays)
        {
            holidays.RemoveAll(d => d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday);
        }

        // Apply bridging (Mon/Fri are holidays if Tue/Thu are)
        private static void ApplyBridging(List<LocalDate> holidays)
        {
            var additionalMondays = holidays.Where(d => d.DayOfWeek == DayOfWeek.Tuesday && !(d.Month == 1 && d.Day == 1))
                                            .Select(d => d.PlusDays(-1))
                                            .ToList();
            var additionalFridays = holidays.Where(d => d.DayOfWeek == DayOfWeek.Thursday && !(d.Month == 12 && d.Day == 26))
                                            .Select(d => d.PlusDays(-1))
                                            .ToList();
            holidays.AddRange(additionalMondays);
            holidays.AddRange(additionalFridays);
        }

        // Calculate Easter day by Delambre
        private static LocalDate Easter(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;
            return new LocalDate(year, month, day);
        }
    }
}
