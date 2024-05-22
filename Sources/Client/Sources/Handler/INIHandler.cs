using System;

namespace SkySpeed.Handler
{
    public class INIHandler
    {
        public const string SkySpeedIni = "SkySpeed.ini";
        public const string FlightDetailsTxt = "Flight_Details.txt";

        public static string INIFilePath { get; set; }
        public static string FlightDetailsPath { get; set; }

        public INIHandler()
        {
            INIFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SkySpeedIni);
            FlightDetailsPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FlightDetailsTxt);
        }
    }
}