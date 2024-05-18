using SkySpeedService.ExecuteQuery;
using SkySpeedService.Handler;
using System.Collections.Generic;

namespace SkySpeedService.DatabaseAndTables
{
    class DBSetup : ExecuteQueries
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
                  $"USER_ID VARCHAR(255) NOT NULL," +
                  $"EMP_NAME VARCHAR(255) NOT NULL," +
                  $"EMP_PASSWORD VARCHAR(255) NOT NULL);",

                $"CREATE TABLE AVAILABILITY_REQUEST (" +
                $"AVAILABILITY_REQUEST_ID INT PRIMARY KEY IDENTITY(1,1)," +
                $"ONEWAY_ROUNDTRIP_DATE VARCHAR(255)," +
                $"MARKET_FROM VARCHAR(255)," +
                $"MARKET_TO VARCHAR(255)," +
                $"OUTBOUNDFLIGHT_DATE VARCHAR(255)," +
                $"OUTBOUNDFLIGHT_TIME VARCHAR(255)," +
                $"RETURNFLIGHT_DATE VARCHAR(255)," +
                $"RETURNFLIGHT_TIME VARCHAR(255)," +
                $"QUERY_ADT INT NOT NULL," +
                $"QUERY_CHD INT NOT NULL); ",

                $"CREATE TABLE FLIGHT_RESULTS (" +
                $"FLIGHT_ID INT PRIMARY KEY IDENTITY(1,1)," +
                $"FLIGHT_NUMBER VARCHAR(255) NOT NULL," +
                $"DESIGNATOR VARCHAR(255) NOT NULL," +
                $"DAY_DATE VARCHAR(255) NOT NULL," +
                $"SECTOR VARCHAR(255) NOT NULL," +
                $"DEPART_ARRIVAL VARCHAR(255) NOT NULL," +
                $"STOP VARCHAR(255) NOT NULL," +
                $"SEATSLEFT VARCHAR(255) NOT NULL," +
                $"FARE VARCHAR(255) NOT NULL," +
                $"DURATION VARCHAR(255) NOT NULL);",

                $"CREATE TABLE PASSENGERS (" +
                $"PASSENGER_ID INT PRIMARY KEY IDENTITY(1,1)," +
                $"FLIGHT_ID INT NOT NULL," +
                $"TYPE VARCHAR(255) NOT NULL," +
                $"LASTNAME VARCHAR(255) NOT NULL," +
                $"FIRSTNAME VARCHAR(255) NOT NULL," +
                $"MIDDLENAME VARCHAR(255)," +
                $"TITLE VARCHAR(255) NOT NULL," +
                $"GENDER VARCHAR(255) NOT NULL," +
                $"DOB VARCHAR(255)," +
                $"NATIONALITY VARCHAR(255)," +
                $"COUNTRY VARCHAR(255)," +
                $"FOREIGN KEY (FLIGHT_ID) REFERENCES FLIGHT_RESULTS (FLIGHT_ID));",

                $"CREATE TABLE CONTACT (" +
                $"CONTACT_ID INT PRIMARY KEY IDENTITY(1,1)," +
                $"PASSENGER_ID INT NOT NULL," +
                $"ADDRESS_LINE1 VARCHAR(255)," +
                $"ADDRESS_LINE2 VARCHAR(255)," +
                $"ADDRESS_POSTAL VARCHAR(255)," +
                $"ADDRESS_TOWN VARCHAR(255)," +
                $"ADDRESS_STATE VARCHAR(255)," +
                $"ADDRESS_COUNTRY VARCHAR(255)," +
                $"MOBILE VARCHAR(255) NOT NULL," +
                $"EMAIL VARCHAR(255) NOT NULL," +
                $"FOREIGN KEY (PASSENGER_ID) REFERENCES PASSENGERS (PASSENGER_ID)); ",

                $"CREATE TABLE SEAT_MAP (" +
                $"SEAT_ID INT PRIMARY KEY IDENTITY(1,1)," +
                $"PASSENGER_ID INT NOT NULL," +
                $"SEAT_FEE VARCHAR(255) NOT NULL," +
                $"FOREIGN KEY (PASSENGER_ID) REFERENCES PASSENGERS (PASSENGER_ID)); ",

                $"CREATE TABLE PAYMENT (" +
                $" PAYMENT_ID INT PRIMARY KEY IDENTITY(1,1)," +
                $"PASSENGER_ID INT NOT NULL," +
                $"PAYMENT_METHOD VARCHAR(255) NOT NULL," +
                $"CARDNUMBER VARCHAR(255)," +
                $"AMOUNT VARCHAR(255) NOT NULL," +
                $"EXPIRATIONDATE VARCHAR(255)," +
                $"CARDHOLDERNAME VARCHAR(255)," +
                $"FOREIGN KEY (PASSENGER_ID) REFERENCES PASSENGERS (PASSENGER_ID));",

                $"CREATE TABLE END_RECORD (" +
                $"END_RECORD_ID INT PRIMARY KEY IDENTITY(1,1)," +
                $"PASSENGER_ID INT NOT NULL," +
                $"ACTION_PERFORM VARCHAR(255)," +
                $"SEND_ITINERARY_VIA VARCHAR(255) NOT NULL," +
                $"FOREIGN KEY (PASSENGER_ID) REFERENCES PASSENGERS (PASSENGER_ID)); "
        };

            foreach (var tableQuery in tableCreationQueries)
            {
                if (isDBTablesCreated)
                    isDBTablesCreated = ExecuteNonQuery(tableQuery, DatabaseHandler.DBParam);
                else
                    break;
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