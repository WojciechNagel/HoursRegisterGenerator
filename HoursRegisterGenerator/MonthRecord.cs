using System;
using System.Collections.Generic;
using System.Linq;

namespace HoursRegisterGenerator
{
    public class MonthRecord
    {
        public int Month { get; }
        public int Year { get; }
        public int CountOfAllDaysInMonth { get; }
        public int CountOfAllWorkingDaysInMonth { get; }
        public List<int> AbsentDaysInMonth { get; }

        public int HoursInMonth => (CountOfAllWorkingDaysInMonth - AbsentDaysInMonth.Count) * 8;

        public List<Day> AllDaysInMonth { get; private set; }

        public MonthMatrix MonthMatrix { get; set; }

        public static int GetCountOfWorkingDaysInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            var dates = new List<DateTime>();
            for (int i = 1; i <= days; i++)
            {
                dates.Add(new DateTime(year, month, i));
            }

            int weekDays = dates.Where(d => d.DayOfWeek > DayOfWeek.Sunday & d.DayOfWeek < DayOfWeek.Saturday).Count();
            return weekDays;
        }

        public override string ToString()
        {
            string monthMatrix = MonthMatrix.Header;

            //TODO: ALL!

            InitializeTableRows();
            monthMatrix += MonthMatrix.RowsString;
            monthMatrix += MonthMatrix.SumString;
            monthMatrix += MonthMatrix.Total;
            monthMatrix += MonthMatrix.Footer;

            return monthMatrix;
        }

        private void InitializeTableRows()
        {
            // FILL START
            var firstDay = AllDaysInMonth[0].DayOfWeek;
            var current = 1; // Monday
            while (current != firstDay)
            {
                MonthMatrix.Rows[current - 1].Add(new Day());
                current++;
            }

            // INITIALIZE MONTH DAYS
            foreach (var day in AllDaysInMonth)
            {
                MonthMatrix.Rows[day.DayOfWeek - 1].Add(day);
            }

            // FILL END
            var last = AllDaysInMonth[AllDaysInMonth.Count - 1].DayOfWeek;
            current = last + 1;
            while (current <= 7) // To Sunday
            {
                MonthMatrix.Rows[current - 1].Add(new Day());
                current++;
            }
        }

        public List<Day> InitializeAllDaysInMonth()
        {
            var daysInMonth = new List<Day>();

            List<int> hoursForDays = Utils.NormalizeRandomWorkHours(
                Utils.GenerateIndicatedAmountOfRandomNumbersWithSpecifiedSum(
                    CountOfAllWorkingDaysInMonth - AbsentDaysInMonth.Count, HoursInMonth).ToList(), 13, 3);

            int workDayInMonth = 0;
            for (int dayInMonth = 1; dayInMonth <= CountOfAllDaysInMonth; dayInMonth++)
            {
                int dayOfWeek = Day.GetDayOfWeekByDate(Year, Month, dayInMonth);
                if (dayOfWeek == 7 || dayOfWeek == 6 || AbsentDaysInMonth.ToList().Contains(dayInMonth)) // Sun, Sat
                    daysInMonth.Add(new Day(Year, Month, dayInMonth, dayOfWeek, 0));
                else
                {
                    daysInMonth.Add(new Day(Year, Month, dayInMonth, dayOfWeek, hoursForDays[workDayInMonth]));
                    workDayInMonth++;
                }
            }

            return daysInMonth;
        }

        public MonthRecord(int month, int year, List<int> absentDaysInMonth)
        {
            Month = month;
            Year = year;
            CountOfAllDaysInMonth = DateTime.DaysInMonth(Year, Month);
            CountOfAllWorkingDaysInMonth = GetCountOfWorkingDaysInMonth(Year, Month);
            AbsentDaysInMonth = absentDaysInMonth ?? new List<int>();
            AllDaysInMonth = InitializeAllDaysInMonth();
            MonthMatrix = new MonthMatrix(HoursInMonth);
        }
    }
}
