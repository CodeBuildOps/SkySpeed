using System.Collections.Generic;

namespace SkySpeedService.Handler
{
    class DatabaseHandler
    {
        private INIHandler _iniFile;
        public const string DATABASENAME = "SKYSPEED";
        private string ServerUserId { get; set; }
        private string ServerName { get; set; }
        private string ServerPassword { get; set; }
        public static string MasterDBParam { get; set; }
        public static string DBParam { get; set; }

        public DatabaseHandler(string iniFilePath)
        {
            _iniFile = new INIHandler(iniFilePath);
        }

        public DatabaseHandler(string serverUserId, string serverName, string serverPassword, string iniFilePath)
        {
            ServerUserId = serverUserId;
            ServerName = serverName;
            ServerPassword = serverPassword;
            _iniFile = new INIHandler(iniFilePath);

            DBParam = $"server={ServerName};initial catalog={DATABASENAME};user id={ServerUserId};password={ServerPassword}";
            MasterDBParam = $"server={ServerName};initial catalog=master;user id={ServerUserId};password={ServerPassword}";
        }

        public bool CreateINI()
        {
            var entries = new List<(string Key, string Value)>
            {
                ("SERVERUSERID", ServerUserId),
                ("SERVERNAME", ServerName),
                ("SERVERPASSWORD", ServerPassword),
                ("DATABASE", DATABASENAME),
                ("MASTERDBPARAM", MasterDBParam),
                ("DBPARM", DBParam)
            };

            bool isIniCreated = true;

            foreach (var (key, value) in entries)
            {
                if (!_iniFile.Write("DATABASE", key, value))
                {
                    isIniCreated = false;
                    break;
                }
            }

            return isIniCreated;
        }

        public Dictionary<string, string> ReadINI()
        {
            var iniValues = new Dictionary<string, string>();

            ServerUserId = iniValues["SERVERUSERID"] = _iniFile.Read("DATABASE", "SERVERUSERID");
            ServerName = iniValues["SERVERNAME"] = _iniFile.Read("DATABASE", "SERVERNAME");
            ServerPassword = iniValues["SERVERPASSWORD"] = _iniFile.Read("DATABASE", "SERVERPASSWORD");
            MasterDBParam = iniValues["MASTERDBPARAM"] = _iniFile.Read("DATABASE", "MASTERDBPARAM");
            DBParam = iniValues["DBPARM"] = _iniFile.Read("DATABASE", "DBPARM");

            return iniValues;
        }
    }
}