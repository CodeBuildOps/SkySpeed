using SkySpeedService.ExecuteQuery;
using SkySpeedService.Handler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkySpeedService.Flight
{
    class FlightDetails : ExecuteQueries
    {
        private const string FLIGHT_RESULTS_TABLE = "FLIGHT_RESULTS";
        public Dictionary<int, Dictionary<string, object>> GetAllFlightDetails()
        {
            string query = $"SELECT * FROM {FLIGHT_RESULTS_TABLE};";
            return ExecuteReaderQuery(query, DatabaseHandler.DBParam);
        }

        public bool InsertFlightDetails(string flightDetailFilePath)
        {
            // Read flight details values from the text file
            var lines = File.ReadAllLines(flightDetailFilePath);

            // Create the insert query
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.AppendLine($"INSERT INTO {FLIGHT_RESULTS_TABLE} (");
            insertQuery.AppendLine("    FLIGHT_NUMBER,");
            insertQuery.AppendLine("    DESIGNATOR,");
            insertQuery.AppendLine("    DAY_DATE,");
            insertQuery.AppendLine("    SECTOR,");
            insertQuery.AppendLine("    DEPART_ARRIVAL,");
            insertQuery.AppendLine("    STOP,");
            insertQuery.AppendLine("    SEATSLEFT,");
            insertQuery.AppendLine("    FARE,");
            insertQuery.AppendLine("    DURATION");
            insertQuery.AppendLine(") VALUES ");

            // Process each line and add it to the query
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    insertQuery.Append(line);
                    if (i < lines.Length - 1)
                    {
                        insertQuery.AppendLine(",");
                    }
                    else
                    {
                        insertQuery.AppendLine(";");
                    }
                }
            }

            string query = insertQuery.ToString();
            return ExecuteNonQuery(query, DatabaseHandler.DBParam);
        }
    }
}