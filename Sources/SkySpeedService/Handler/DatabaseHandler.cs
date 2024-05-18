using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkySpeedService.Handler
{
    class DatabaseHandler
    {
        public const string DATABASENAME = "SKYSPEED";
        private string ServerUserId { get; set; }
        private string ServerName { get; set; }
        private string ServerPassword { get; set; }
        private string INIFilePath { get; set; }
        public static string MasterDBParam { get; set; }
        public static string DBParam { get; set; }

        public DatabaseHandler(string iniFilePath)
        {
            INIFilePath = iniFilePath;
        }

        public DatabaseHandler(string serverUserId, string serverName, string serverPassword, string iniFilePath)
        {
            ServerUserId = serverUserId;
            ServerName = serverName;
            ServerPassword = serverPassword;
            INIFilePath = iniFilePath;

            DBParam = $"server={ServerName};initial catalog={DATABASENAME};user id={ServerUserId};password={ServerPassword}";
            MasterDBParam = $"server={ServerName};initial catalog=master;user id={ServerUserId};password={ServerPassword}";
        }

        public bool CreateINI()
        {
            bool isIniCreated = false;
            using (StreamWriter writer = new StreamWriter(INIFilePath))
            {
                StringBuilder content = new StringBuilder();
                content.AppendLine($"[DATABASE]");
                content.AppendLine($"SERVERUSERID={ServerUserId}");
                content.AppendLine($"SERVERNAME={ServerName}");
                content.AppendLine($"SERVERPASSWORD={ServerPassword}");
                content.AppendLine($"DATABASE={DATABASENAME}");
                content.AppendLine($"MASTERDBPARAM={MasterDBParam}");
                content.AppendLine($"DBPARM={DBParam}");
                writer.Write(content.ToString());

                isIniCreated = true;
            }
            return isIniCreated;
        }

        public Dictionary<string, string> ReadINI()
        {
            var iniValues = new Dictionary<string, string>();
            using (StreamReader reader = new StreamReader(INIFilePath))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!(line == "[DATABASE]"))
                    {
                        int startIndex = line.IndexOf('=') + 1;
                        iniValues[line.Substring(0, startIndex - 1)] = line.Substring(startIndex).Trim();
                    }
                }
            }

            ServerUserId = iniValues["SERVERUSERID"];
            ServerName = iniValues["SERVERNAME"];
            ServerPassword = iniValues["SERVERPASSWORD"];
            MasterDBParam = iniValues["MASTERDBPARAM"];
            DBParam = iniValues["DBPARM"];

            return iniValues;
        }
    }
}