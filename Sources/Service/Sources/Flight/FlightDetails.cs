using SkySpeedService.ExecuteQuery;
using SkySpeedService.Handler;
using System.Collections.Generic;

namespace SkySpeedService.Flight
{
    class FlightDetails : ExecuteQueries
    {
        public Dictionary<int, Dictionary<string, object>> GetAllFlightDetails()
        {
            string query = $"SELECT * FROM FLIGHT_RESULTS;";
            return ExecuteReaderQuery(query, DatabaseHandler.DBParam);
        }
    }
}
