using System;
using System.Collections.Generic;
using System.Linq;

namespace HoursRegisterGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new Config(args);

            ConfigGlobalValues.Activities = config.Activities;
            var generator = new HoursRegisterGenerator(config);
            generator.GenerateHoursRegister();
            generator.SaveHoursRegister(config.ReportDestination);
        }
    }
}
