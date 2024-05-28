namespace SkySpeedService.PNR
{
    class HTMLBody
    {
        public const string _htmlBody = @"
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
                                    <th>Passenger name:</th>
                                    <td>John Doe</td>
                                </tr>
                                <tr>
                                    <th>Take-off:</th>
                                    <td>2024-06-15, 10:00 AM <br>
				                    JFK International Airport, New York</td>
                                </tr>
                                <tr>
                                    <th>Landing:</th>
                                    <td>2024-06-15, 12:15 PM <br>
				                    LAX International Airport, Los Angeles</td>
                                </tr>
			                    <tr>
                                    <th>Flight duration:</th>
                                    <td>2 hrs, 15 mins</td>
                                </tr>
			                     <tr>
                                    <th>Flight number:</th>
                                    <td>AB1234</td>
                                </tr>
                                <tr>
                                    <th>Seat:</th>
                                    <td>12A</td>
                                </tr>
                            </table>
                            <div class=""pnr"">
			                    <h3>
				                    PNR: ABCD1234
			                    </h3>
                            </div>
                            <div class=""footer"">
                                <p>Have a safe and pleasant journey!</p>
                            </div>
                        </div>
                    </body>
                    </html>
        ";
    }
}