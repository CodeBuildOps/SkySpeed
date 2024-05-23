using System;
using System.Net;
using System.Net.Mail;

namespace SkySpeedService.Email
{
    class SendEmail
    {
        private const string fromMail = "abhiksingh1999@gmail.com"; /*"skyspeed.cu@gmail.com";*/
        private const string fromPassword = "uqha jdiw knbo bawu";
        private const string mailBody = @"
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
        private readonly string toMail;

        public SendEmail(string toMail)
        {
            this.toMail = toMail;
        }

        public bool IsEmailSent()
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(fromMail);
                    message.Subject = "E-Ticket for your flight PNR: ABCD1234";
                    message.To.Add(new MailAddress(toMail));
                    message.Body = mailBody;
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                        smtpClient.EnableSsl = true;

                        smtpClient.Send(message);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}