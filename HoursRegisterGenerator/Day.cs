using System;
using System.Collections.Generic;
using System.Linq;

namespace HoursRegisterGenerator
{
    public class Day
    {
        private const int MAX_COUNT_OF_ACTIVITIES_PER_DAY = 4;

        public readonly List<string> AvailableActivities = ConfigGlobalValues.Activities;

        public int Month { get; }
        public int Year { get; }
        public int DayOfWeek { get; }
        public int DayInMonth { get; }
        public int WorkHours { get; }
        public bool IsWorkingDay { get; }
        public List<(int, string)> Activities { get; set; } = new List<(int, string)>();

        public static int GetDayOfWeekByDate(int year, int month, int dayInMonth)
        {
            var dayOfWeek = new DateTime(year, month, dayInMonth).DayOfWeek;
            if (dayOfWeek == System.DayOfWeek.Sunday)
                return 7;
            return (int)dayOfWeek;
        }

        public static bool IsItWorkingDay(int dayOfWeek)
        {
            return !new[] { 6, 7 }.Contains(dayOfWeek);
        }

        public override string ToString()
        {
            if (!IsWorkingDay)
            {
                return @$"
            <td class=""day"">{(DayInMonth == 0 ? "" : DayInMonth.ToString())}</td>
            <td class=""idle_day"">&nbsp;</td>";
            }

            return @$"
            <td class=""day"">{DayInMonth}</td>
            <td class=""work_day"">
                <table>{GenerateActivitiesTableContent()}
                </table>
            </td>";
        }

        public string GenerateActivitiesTableContent()
        {
            string tableContent = "";
            foreach (var activity in Activities)
            {
                tableContent += $@"
                    <tr>
                        <td>{activity.Item1}</td>
                        <td>{activity.Item2}</td>
                    </tr>";
            }
            return tableContent;
        }

        public List<(int, string)> InitializeActivities(int workHours)
        {
            //int maxCountOfActivities = AvailableActivities.Count() > workHours ? workHours : AvailableActivities.Count();
            int maxCountOfActivities = MAX_COUNT_OF_ACTIVITIES_PER_DAY; // IF YOU DON'T WANT TO USE CONSTANT, USE LINE ABOVE
            int countOfActivities = Utils.GetRandomNumber(1, maxCountOfActivities);

            var hoursForActivities = Utils.NormalizeRandomWorkHours(
                Utils.GenerateIndicatedAmountOfRandomNumbersWithSpecifiedSum(countOfActivities, workHours).ToList(), workHours, 1);
            var activities = new List<(int, string)>();
            var activitiesUsed = new List<int>();
            foreach (var hoursOfActivity in hoursForActivities)
            {
                int numberOfActivity;
                do
                {
                    numberOfActivity = Utils.GetRandomNumber(0, AvailableActivities.Count - 1);
                    if (!activitiesUsed.Contains(numberOfActivity))
                    {
                        activitiesUsed.Add(numberOfActivity);
                        break;
                    }
                }
                while (true);
                activities.Add((hoursOfActivity, AvailableActivities[numberOfActivity]));
            }
            return activities;
        }

        public Day(int year, int month, int dayInMonth, int dayOfWeek, int workHours)
        {
            Year = year;
            Month = month;
            DayInMonth = dayInMonth;
            DayOfWeek = dayOfWeek;
            WorkHours = workHours;
            IsWorkingDay = IsItWorkingDay(dayOfWeek) && workHours != 0;

            if (IsWorkingDay)
                Activities = InitializeActivities(WorkHours);
        }

        public Day()
        {
            IsWorkingDay = false;
        }
    }
}
