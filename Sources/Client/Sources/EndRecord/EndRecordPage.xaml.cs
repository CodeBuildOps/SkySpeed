using SharedData;
using SkySpeed.FlightResults;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using SkySpeedService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkySpeed.EndRecords
{
    /// <summary>
    /// Interaction logic for EndRecordPage.xaml
    /// </summary>
    public partial class EndRecordPage : Page
    {
        private readonly DisplayMessage _displayMessage;
        private readonly SkySpeedServices _skySpeedServices;
        private const string _docxFile = "PNR.docx";
        private string _toMail;
        private readonly Dictionary<string,string> _passengerSeatsNames;
        private string _pnr;
        private string _flightNumber;
        private string _flightDuration;
        private string _takeOff;
        private string _takeOffAirport;
        private string _landing;
        private string _landingAirport;

        public EndRecordPage()
        {
            InitializeComponent();

            _passengerSeatsNames = new Dictionary<string, string>();
            _displayMessage = new DisplayMessage("End record");
            _skySpeedServices = new SkySpeedServices();
            GetEmailId();
            GenerateRandomPNR(6);
            GetAllPassengersDetails();
            GetFlightDetails();
        }

        private void GetEmailId()
        {
            // Todo: Get email id from datagrid.
            _toMail = "abhiksingh1999@gmail.com";
        }

        private void SendItineraryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(SendItineraryComboBox.SelectedItem is ContentControl selectedItemControl))
                return;

            Cursor = Cursors.Wait;
            string htmlContent = _skySpeedServices.GenerateHtml(
                _passengerSeatsNames,
                _takeOff,
                _takeOffAirport,
                _landing,
                _landingAirport,
                _flightDuration,
                _flightNumber,
                _pnr
                );
            switch (selectedItemControl.Content.ToString().ToUpper())
            {
                case "EMAIL":
                    HandleEndRecordSelection("Would you like to send an email?", () => _skySpeedServices.SendEmail(_toMail, htmlContent));
                    break;
                case "PRINT":
                    HandleEndRecordSelection("Would you like to print?", () => _skySpeedServices.PrintDocument(Path.Combine(Environment.CurrentDirectory, _docxFile), htmlContent));
                    break;
            }
            InsertRecordsInTables();
            Cursor = Cursors.Arrow;
        }

        // Todo: Add Flight details object and PNR details in the Passenger grid
        private bool InsertRecordsInTables()
        {
            var details = new Dictionary<string, List<string>>
            {
                ["FLIGHT"] = new List<string> { _flightNumber },
                ["PNR"] = new List<string> { _pnr }
            };

            foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
            {
                if (rowItem is PassengersDetails row && row.PaymentDetailsObject != null)
                {
                    details["PASSENGERS"] = new List<string>
                    {
                        row.PassengerId.ToString(),
                        row.Type,
                        row.FullName,
                        row.Gender,
                        row.DOB,
                        row.Country
                    };
                    details["CONTACT"] = new List<string>
                    {
                        row.Mobile,
                        row.Email,
                        row.FullAddress
                    };
                    details["SEAT"] = new List<string>
                    {
                        row.Seat,
                        row.SeatPrice.ToString()
                    };
                    details["PAYMENT"] = new List<string>
                    {
                        row.PaymentDetailsObject.PaymentMethod,
                        row.PaymentDetailsObject.Amount
                    };

                    if (!_skySpeedServices.InsertRecordsInTables(details))
                    {
                        _displayMessage.ShowErrorMessageBox($"Error while inserting the record into the database. Contact support.");
                        return false;
                    }
                }
            }

            return true;
        }

        private void GenerateRandomPNR(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] pnrArray = new char[length];

            for (int i = 0; i < length; i++)
            {
                pnrArray[i] = chars[random.Next(chars.Length)];
            }

            _pnr = new string(pnrArray);
        }

        private void GetAllPassengersDetails()
        {
            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
            {
                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        _passengerSeatsNames[row.Seat] = row.FullName;
                    }
                }
            }
        }

        private void GetFlightDetails()
        {
            if (SharedDataPage.FlightDetails is FlightDetails flightDetails)
            {
                _flightNumber = flightDetails.FlightNumber;
                _flightDuration = flightDetails.Duration;

                _takeOff = $"{flightDetails.DayAndDate.Trim()}, {flightDetails.DepartArrival.Split('-')[0].Trim()}";
                _takeOffAirport = flightDetails.Sector.Split('-')[0];

                _landing = $"{flightDetails.DayAndDate.Trim()}, {flightDetails.DepartArrival.Split('-')[1].Trim()}";
                _landingAirport = flightDetails.Sector.Split('-')[1];
            }
        }

        private void HandleEndRecordSelection(string message, Action action)
        {
            if (_displayMessage.ShowQuestionMessageBox(message) == MessageBoxResult.Yes)
            {
                action();
            }
            SendItineraryComboBox.SelectedItem = null;
        }
    }
}