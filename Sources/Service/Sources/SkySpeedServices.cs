using SkySpeedService.DatabaseAndTables;
using SkySpeedService.Email;
using SkySpeedService.Flight;
using SkySpeedService.Handler;
using SkySpeedService.SignupAndLogin;
using System.Collections.Generic;
using SkySpeedService.Print;
using SkySpeedService.PNR;

namespace SkySpeedService
{
    public class SkySpeedServices
    {
        private DatabaseHandler _dbHandler;

        public bool CreateINI(string serverUserId, string serverName, string serverPassword, string iniFilePath)
        {
            _dbHandler = new DatabaseHandler(serverUserId, serverName, serverPassword, iniFilePath);
            return _dbHandler.CreateINI();
        }

        public bool DatabaseExists(string iniFilePath)
        {
            _dbHandler = new DatabaseHandler(iniFilePath);
            _dbHandler.ReadINI();

            var dBSetup = new DBSetup();
            return dBSetup.IsDatabaseExist();
        }

        public bool CreateDBSetup()
        {
            var dBSetup = new DBSetup();
            return dBSetup.CreateDBSetup();
        }

        public bool CreateRegistration(string userId, string empPassword, string empName)
        {
            var registration = new RegistrationAndLogin(userId, empPassword, empName);
            return registration.CreateRegistration();
        }

        public bool InsertFlightDetails(string flightDetailFilePath)
        {
            var flightDetails = new FlightDetails();
            return flightDetails.InsertFlightDetails(flightDetailFilePath);
        }

        public bool DoLogin(string userId, string empPassword)
        {
            var login = new RegistrationAndLogin(userId, empPassword, null);
            return login.DoLogin();
        }

        public Dictionary<int, Dictionary<string, object>> GetAllFlightDetails()
        {
            var flightDetails = new FlightDetails();
            return flightDetails.GetAllFlightDetails();
        }

        public string GenerateHtml(Dictionary<string, string> passengerDetails, string takeOff, string takeOffAirport, string landing, string landingAirport, string flightDuration, string flightNumber, string pnr)
        {
            var htmlBody = new HTMLBody();
            return htmlBody.GenerateHtml(passengerDetails, takeOff, takeOffAirport, landing, landingAirport, flightDuration, flightNumber, pnr);
        }

        public bool PrintDocument(string pnrFilePath, string htmlContent)
        {
            var documentPrinter = new DocumentPrinter();
            return documentPrinter.PrintWordDocument(pnrFilePath, htmlContent);
        }

        public bool SendEmail(string toMail, string htmlContent)
        {
            var sendEmail = new SendEmail();
            return sendEmail.EmailSend(toMail, htmlContent);
        }

        public bool InsertRecordsInTables(Dictionary<string, List<string>> details)
        {
            var insertRecordsInTables = new InsertRecordsInTables();
            insertRecordsInTables.InsertRecords(details);
            return true;
        }
    }
}