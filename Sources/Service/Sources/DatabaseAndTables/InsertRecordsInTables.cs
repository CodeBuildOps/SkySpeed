using SkySpeedService.ExecuteQuery;
using SkySpeedService.Handler;
using System.Collections.Generic;

namespace SkySpeedService.DatabaseAndTables
{
    internal class InsertRecordsInTables : ExecuteQueries
    {
        public bool InsertRecords(Dictionary<string, List<string>> details)
        {
            // Insert all passenger details into the Passenger table
            string flightNumber = null;
            string passengerId = null, type = null, fullName = null, gender = null, dob = null, country = null;
            string mobile = null, email = null, fullAddress = null;
            string seat = null, seatPrice = null;
            string paymentMethod = null, amount = null;
            string pnr = null;

            foreach (KeyValuePair<string, List<string>> entry in details)
            {
                switch (entry.Key)
                {
                    case "FLIGHT":
                        flightNumber = entry.Value[0];
                        break;
                    case "PASSENGERS":
                        passengerId = entry.Value[0];
                        type = entry.Value[1];
                        fullName = entry.Value[2];
                        gender = entry.Value[3];
                        dob = entry.Value[4];
                        country = entry.Value[5];
                        break;
                    case "CONTACT":
                        mobile = entry.Value[0];
                        email = entry.Value[1];
                        fullAddress = entry.Value[2];
                        break;
                    case "SEAT":
                        seat = entry.Value[0];
                        seatPrice = entry.Value[1];
                        break;
                    case "PAYMENT":
                        paymentMethod = entry.Value[0];
                        amount = entry.Value[1];
                        break;
                    case "PNR":
                        pnr = entry.Value[0];
                        break;
                }
            }

            string query = $"INSERT INTO PASSENGERS (" +
                $"PNR, FLIGHT_NUMBER, PASSENGER_ID, TYPE, FULLNAME, GENDER, DOB, COUNTRY, MOBILE, EMAIL, FULLADDRESS, SEAT, SEAT_PRICE, PAYMENT_METHOD, AMOUNT) VALUES(" +
                $"'{pnr}', '{flightNumber}', '{passengerId}', '{type}', '{fullName}', '{gender}', '{dob}', '{country}', '{mobile}', '{email}', '{fullAddress}', '{seat}', '{seatPrice}', '{paymentMethod}', '{amount}');";

            return ExecuteNonQuery(query, DatabaseHandler.DBParam);
        }
    }
}