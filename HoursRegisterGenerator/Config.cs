using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoursRegisterGenerator
{
    public class Config
    {
        public string CompanyName { get; private set; }
        public string NameAndSurname { get; private set; }
        public string TeamLeader { get; private set; }
        public string EmailAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int MaxCountOfActivitiesPerDay { get; private set; }
        public List<int> DaysOfAbsence { get; private set; }
        public string ReportDestination { get; private set; }
        public List<string> Activities { get; private set; }
        

        private IConfiguration _configuration;

        private void BuildConfiguration(string[] args)
        {
            const string configurationFile = "appsettings.json";
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(configurationFile, optional: false)
                .AddCommandLine(args)
                .Build();
        }

        private void ReadConfiguration()
        {
            Console.WriteLine("Configuration:" );

            CompanyName = _configuration.GetValue<string>(nameof(CompanyName));
            Console.WriteLine($"{nameof(CompanyName)}: {CompanyName}");

            NameAndSurname = _configuration.GetValue<string>(nameof(NameAndSurname));
            Console.WriteLine($"{nameof(NameAndSurname)}: {NameAndSurname}");

            TeamLeader = _configuration.GetValue<string>(nameof(TeamLeader));
            Console.WriteLine($"{nameof(TeamLeader)}: {TeamLeader}");

            EmailAddress = _configuration.GetValue<string>(nameof(EmailAddress));
            Console.WriteLine($"{nameof(EmailAddress)}: {EmailAddress}");

            PhoneNumber = _configuration.GetValue<string>(nameof(PhoneNumber));
            Console.WriteLine($"{nameof(PhoneNumber)}: {PhoneNumber}");

            Year = _configuration.GetValue<int>(nameof(Year));
            Console.WriteLine($"{nameof(Year)}: {Year}");

            Month = _configuration.GetValue<int>(nameof(Month));
            Console.WriteLine($"{nameof(Month)}: {Month}");

            MaxCountOfActivitiesPerDay = _configuration.GetValue<int>(nameof(MaxCountOfActivitiesPerDay));
            Console.WriteLine($"{nameof(MaxCountOfActivitiesPerDay)}: {MaxCountOfActivitiesPerDay}");

            DaysOfAbsence = _configuration.GetSection(nameof(DaysOfAbsence)).Get<List<int>>();
            if (DaysOfAbsence != null) Console.WriteLine($"{nameof(DaysOfAbsence)}: {string.Join(", ", DaysOfAbsence)}");
            else Console.WriteLine($"{nameof(DaysOfAbsence)}: ");

            ReportDestination = _configuration.GetValue<string>(nameof(ReportDestination));
            Console.WriteLine($"{nameof(ReportDestination)}: {ReportDestination}");

            Activities = _configuration.GetSection(nameof(Activities)).Get<List<string>>();
            Console.WriteLine($"{nameof(Activities)}:\n - {string.Join(",\n - ", Activities)}");
        }

        private bool ShouldDisplayHelp(string[] args)
        {
            string[] helpArguments = new string[] {
                "-h", "-help",
                "/h", "/help",
                "--h", "--help"
            };
            return args.Any(a => helpArguments.Contains(a));
        }

        private void DisplayHelpAndExit()
        {
            string helpGeneral = "HoursRegisterGenerator.exe\n"
                + "This is an fucking awesome app to create a record of hours. ";

            string helpParameters = @"Available parameters (in command line format):
--Year=
--Month=
--DaysOfAbsence=[ 1, 2, 3, 4, 5, 6]";

            Console.WriteLine(helpGeneral);
            Console.WriteLine(helpParameters);
            Environment.Exit(0);
        }

        public Config(string[] args)
        {
            if (ShouldDisplayHelp(args))
                DisplayHelpAndExit();

            BuildConfiguration(args);
            ReadConfiguration();
        }
    }
}
