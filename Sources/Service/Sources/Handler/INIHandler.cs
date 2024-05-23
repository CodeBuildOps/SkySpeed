using System.Runtime.InteropServices;
using System.Text;

namespace SkySpeedService.Handler
{
    class INIHandler
    {
        private readonly string INIFilePath;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public INIHandler(string iniFilePath)
        {
            INIFilePath = iniFilePath;
        }

        public bool Write(string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, INIFilePath) != 0;
        }

        public string Read(string section, string key, string defaultValue = "")
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, retVal, 255, INIFilePath);
            return retVal.ToString();
        }
    }
}
