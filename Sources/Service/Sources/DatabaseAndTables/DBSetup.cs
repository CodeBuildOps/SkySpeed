using SkySpeedService.ExecuteQuery;
using SkySpeedService.Handler;
using System.Collections.Generic;

namespace SkySpeedService.DatabaseAndTables
{
    internal class DBSetup : ExecuteQueries
    {
        public bool CreateDBSetup()
        {
            bool isDBTablesCreated = true;

            // Create SKYSPEED Database
            string query = $"CREATE DATABASE {DatabaseHandler.DATABASENAME};";
            isDBTablesCreated &= ExecuteNonQuery(query, DatabaseHandler.MasterDBParam);

            // Create Tables
            List<string> tableCreationQueries = new List<string>()
            {
                $"CREATE TABLE SIGNUP (" +
                  $"SIGNUP_ID INT PRIMARY KEY IDENTITY(1,1)," +
                  $"USER_ID VARCHAR(10) NOT NULL," +
                  $"EMP_NAME VARCHAR(25) NOT NULL," +
                  $"EMP_PASSWORD VARCHAR(20) NOT NULL);",

                $"CREATE TABLE FLIGHT (" +
                $"FLIGHT_ID INT IDENTITY(1,1)," +
                $"FLIGHT_NUMBER VARCHAR(10) PRIMARY KEY NOT NULL," +
                $"DESIGNATOR VARCHAR(15) NOT NULL," +
                $"DAY_DATE VARCHAR(15) NOT NULL," +
                $"SECTOR VARCHAR(10) NOT NULL," +
                $"DEPART_ARRIVAL VARCHAR(15) NOT NULL," +
                $"STOP VARCHAR(1) NOT NULL," +
                $"SEATSLEFT VARCHAR(2) NOT NULL," +
                $"FARE VARCHAR(10) NOT NULL," +
                $"DURATION VARCHAR(10) NOT NULL);",

                $"CREATE TABLE PASSENGERS (" +
                $"PNR VARCHAR(10)," +
                $"FLIGHT_NUMBER VARCHAR(10) NOT NULL," +
                $"PASSENGER_ID INT NOT NULL," +
                $"TYPE VARCHAR(3) NOT NULL," +
                $"FULLNAME VARCHAR(25)," +
                $"GENDER VARCHAR(6) NOT NULL," +
                $"DOB VARCHAR(10)," +
                $"COUNTRY VARCHAR(15)," +
                $"MOBILE VARCHAR(12)," +
                $"EMAIL VARCHAR(25)," +
                $"FULLADDRESS VARCHAR(255) NOT NULL," +
                $"SEAT VARCHAR(2) NOT NULL," +
                $"SEAT_PRICE VARCHAR(4) NOT NULL," +
                $"PAYMENT_METHOD VARCHAR(15) NOT NULL," +
                $"AMOUNT VARCHAR(15) NOT NULL," +
                $"FOREIGN KEY (FLIGHT_NUMBER) REFERENCES FLIGHT (FLIGHT_NUMBER));"
        };

            foreach (string tableQuery in tableCreationQueries)
            {
                if (isDBTablesCreated)
                {
                    isDBTablesCreated = ExecuteNonQuery(tableQuery, DatabaseHandler.DBParam);
                }
                else
                {
                    break;
                }
            }

            return isDBTablesCreated;
        }

        public bool IsDatabaseExist()
        {
            string query = $"SELECT database_id FROM sys.databases WHERE Name = '{DatabaseHandler.DATABASENAME}';";
            return ExecuteScalar(query, DatabaseHandler.MasterDBParam);
        }
    }
}