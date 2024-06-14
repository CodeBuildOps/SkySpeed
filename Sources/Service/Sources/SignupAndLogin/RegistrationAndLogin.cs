using SkySpeedService.ExecuteQuery;
using SkySpeedService.Handler;
using System.Collections.Generic;

namespace SkySpeedService.SignupAndLogin
{
    internal class RegistrationAndLogin : ExecuteQueries
    {
        private string UserId { get; set; }
        private string EmpPassword { get; set; }
        private string EmpName { get; set; }

        public RegistrationAndLogin(string userId, string empPassword, string empName = null)
        {
            UserId = userId;
            EmpPassword = empPassword;
            EmpName = empName;
        }

        public bool CreateRegistration()
        {
            string query = $"INSERT INTO SIGNUP (USER_ID, EMP_NAME, EMP_PASSWORD) VALUES('{UserId}', '{EmpName}', '{EmpPassword}');";
            return ExecuteNonQuery(query, DatabaseHandler.DBParam);
        }

        public bool DoLogin()
        {
            string query = $"SELECT * FROM SIGNUP WHERE USER_ID = '{UserId}';";

            Dictionary<int, Dictionary<string, object>> getLoginDetails = ExecuteReaderQuery(query, DatabaseHandler.DBParam);
            foreach (KeyValuePair<int, Dictionary<string, object>> kvp in getLoginDetails)
            {
                if (kvp.Value.TryGetValue("EMP_PASSWORD", out object empPassword))
                {
                    if (empPassword.ToString() == EmpPassword)
                    {
                        if (kvp.Value.TryGetValue("EMP_NAME", out object empName))
                        {
                            EmpName = empName.ToString();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}