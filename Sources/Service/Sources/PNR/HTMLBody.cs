using System.Collections.Generic;
using System.Text;

namespace SkySpeedService.PNR
{
    class HTMLBody
    {
        private readonly string HtmlBody = @"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Flight Ticket</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    margin: 20px;
                    padding: 0;
                }
                .ticket-container {
                    border: 2px solid #000;
                    padding: 20px;
                    max-width: 600px;
                    margin: auto;
                    background-color: #f9f9f9;
                }
                .ticket-header {
                    text-align: center;
                    margin-bottom: 20px;
                }
                .ticket-details {
                    width: 100%;
                    border-collapse: collapse;
                }
                .ticket-details th, .ticket-details td {
                    padding: 10px;
                    text-align: left;
                }
                .ticket-details th {
                    width: 40%;
                    font-weight: bold;
                }
                .passenger-list {
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 20px;
                }
                .passenger-list th, .passenger-list td {
                    padding: 10px;
                    text-align: left;
                    border-bottom: 1px solid #ddd;
                }
                .passenger-list th {
                    font-weight: bold;
                }
                .pnr {
                    text-align: center;
                    margin-top: 20px;
                    font-size: 1.2em;
                }
                .footer {
                    text-align: center;
                    margin-top: 20px;
                }
            </style>
        </head>
        <body>
            <div class=""ticket-container"">
                <div class=""ticket-header"">
                    <h1>Flight Ticket</h1>
                    <h2>Airline Name</h2>
                </div>
                <table class=""ticket-details"">
                    <tr>
                        <th>Take-off:</th>
                        <td>{{TakeOff}} <br>
                        {{TakeOffAirport}}</td>
                    </tr>
                    <tr>
                        <th>Landing:</th>
                        <td>{{Landing}} <br>
                        {{LandingAirport}}</td>
                    </tr>
                    <tr>
                        <th>Flight duration:</th>
                        <td>{{FlightDuration}}</td>
                    </tr>
                    <tr>
                        <th>Flight number:</th>
                        <td>{{FlightNumber}}</td>
                    </tr>
                </table>
                <table class=""passenger-list"">
                    <tr>
                        <th>Seat</th>
                        <th>Passenger name</th>
                    </tr>
                    {{PassengerList}}
                </table>
                <div class=""pnr"">
                    <h3>
                        PNR: {{PNR}}
                    </h3>
                </div>
                <div class=""footer"">
                    <p>Have a safe and pleasant journey!</p>
                </div>
            </div>
        </body>
        </html>
    ";

        public string GenerateHtml(Dictionary<string, string> passengersDetails, string takeOff, string takeOffAirport, string landing, string landingAirport, string flightDuration, string flightNumber, string pnr)
        {
            StringBuilder passengerListHtml = new StringBuilder();

            foreach (var passenger in passengersDetails)
            {
                passengerListHtml.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", passenger.Key, passenger.Value);
            }

            string updatedHtml = HtmlBody
                .Replace("{{PassengerList}}", passengerListHtml.ToString())
                .Replace("{{TakeOff}}", takeOff)
                .Replace("{{TakeOffAirport}}", takeOffAirport)
                .Replace("{{Landing}}", landing)
                .Replace("{{LandingAirport}}", landingAirport)
                .Replace("{{FlightDuration}}", flightDuration)
                .Replace("{{FlightNumber}}", flightNumber)
                .Replace("{{PNR}}", pnr);

            return updatedHtml;
        }
    }
}