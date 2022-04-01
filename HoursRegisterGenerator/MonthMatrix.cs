using System.Collections.Generic;
using System.Linq;

namespace HoursRegisterGenerator
{
    public class MonthMatrix
    {
        private const int COLUMNS_COUNT = 12; // It shouldn't matter - the table should change its size dynamically.
        private readonly int _total;

        public readonly string Header = @$"
    <table class=""wide"">
        <tr>
            <th colspan=""{COLUMNS_COUNT}"">EWIDENCJA GODZIN W OKRESIE ROZLICZENIOWYM</th>
        </tr>";

        public string RowsString => RowsToString();

        public string SumString => SumToString();

        public string Total => @$"
        <tr>
            <td colspan=""{COLUMNS_COUNT}"" class=""total"">{_total}</td>
        </tr>";

        public readonly string Footer = @"
    </table>";

        public List<List<Day>> Rows { get; set; } = new List<List<Day>>
            {
                new List<Day>(), // Mon
                new List<Day>(), // Tue
                new List<Day>(), // Wed
                new List<Day>(), // Thr
                new List<Day>(), // Fr
                new List<Day>(), // Sat
                new List<Day>()  // Sun
             };

        private string RowsToString()
        {
            string rowString = "";
            int i = 0;
            foreach (var row in Rows)
            {
                var weekDaysInPolish = new List<string> { "pon", "wt", "sr", "czw", "pt", "sob", "nd" };
                rowString += @"
        <tr>";
                rowString += @$"
            <td class=""day"">{weekDaysInPolish[i]}</td>";

                foreach (var day in row)
                {
                    rowString += day.ToString();
                }

                rowString += @"
        </tr>";
                i++;
            }

            return rowString;
        }

        private string SumToString()
        {
            string sumString = @$"
         <tr>
            <td class=""day"">razem:</td>
            {GenerateSum()}
        </tr>";

            return sumString;
        }

        private string GenerateSum()
        {
            string sumString = "";
            for (int i = 0; i < Rows.First().Count; i++)
            {
                sumString += @$"<td class=""day""></td>
            <td class=""total"">{GetSumValue(i)}</td>";
            }
            return sumString;
        }

        private int GetSumValue(int index)
        {
            int sumOfWorkHours = 0;
            foreach (var row in Rows)
                sumOfWorkHours += row[index].WorkHours;

            return sumOfWorkHours;
        }

        public MonthMatrix(int total)
        {
            _total = total;
        }
    }
}
