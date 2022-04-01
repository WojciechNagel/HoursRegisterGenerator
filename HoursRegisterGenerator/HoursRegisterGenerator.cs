using System;
using System.IO;

namespace HoursRegisterGenerator
{
    public class HoursRegisterGenerator
    {
        private const string header =
@"<html>
<head>
    <style type=""text/css"">
        TH {text-align: center; background-color: darkgray;}
        TABLE {border-collapse: collapse;}
        TABLE,
        TD,
        TH {border: 1px solid black;}
        TD,
        TH {padding: 3px;}
        .bold {font-weight: bold; background-color: lightgray;}
        .day {font-weight: bold; background-color: darkgray;}
        .idle_day {min-width:150px; font-weight: bold; background-color: lightgray;}
        .work_day {min-width:150px;vertical-align: top}
        .total {text-align: center;}
        TABLE.wide {width: 1024px;}
    </style>
</head>
<body>";

        private const string footer = @"
</body>
</html>";

        private string HoursRegister { get; set; }

        private string GenerateContractorTable()
        {
            return @$"
    <table class=""contractor_table"" style=""float: left; margin-right: 50px"">
        <tr>
            <td>Wykonawca:</td>
            <td>{Config.CompanyName}</td>
        </tr>
        <tr>
            <td>Telefon:</td>
            <td>{Config.PhoneNumber}</td>
        </tr>
        <tr>
            <td>Lider zespołu:</td>
            <td>{Config.TeamLeader}</td>
        </tr>
    </table>
    <table class=""contractor_table"" style=""margin-right: 15px"" >
        <tr>
            <td>Adres e-mail:</td>
            <td>{Config.EmailAddress}</td>
        </tr>
        <tr>
            <td>Faktura nr:</td>
            <td>FV/{Month}/{Config.Year}</td>
        </tr>
        <tr>
            <td>Okres rozl:</td>
            <td>{Config.Year}-{Month}</td>
        </tr>
    </table>
    <hr>";
        }

        private string GenerateMonthTable()
        {
            return FullMonth.ToString();
        }

        public MonthRecord FullMonth { get; }
        public string Month { get; }
        public Config Config { get; }

        public string GenerateHoursRegister()
        {
            HoursRegister += header;
            HoursRegister += GenerateContractorTable();
            HoursRegister += GenerateMonthTable();
            HoursRegister += footer;
            return HoursRegister;
        }

        public void SaveHoursRegister(string location)
        {
            string fileName = $"ewg_{Month}_{Config.Year}.html";
            File.WriteAllText(location + fileName, HoursRegister);
        }

        public HoursRegisterGenerator(Config config)
        {
            Config = config;
            Month = config.Month >= 10 ? config.Month.ToString() : $"0{config.Month}";
            FullMonth = new MonthRecord(config.Month, Config.Year, config.DaysOfAbsence);
        }
    }
}
