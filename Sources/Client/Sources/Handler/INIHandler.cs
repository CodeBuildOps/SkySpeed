using System;

namespace SkySpeed.Handler
{
    public class INIHandler
    {
        private const string SKYSPEED_INI = "SkySpeed.ini";
        public static string INIFilePath { get; set; }

        public INIHandler()
        {
            INIFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SKYSPEED_INI);
        }
    }
}
