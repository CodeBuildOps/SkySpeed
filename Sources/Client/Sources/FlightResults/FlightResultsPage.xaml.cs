using SharedData;
using SkySpeed.MessageLog;
using SkySpeedService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SkySpeed.FlightResults
{
    /// <summary>
    /// Interaction logic for FlightResultsPage.xaml
    /// </summary>
    public partial class FlightResultsPage : Page
    {
        private readonly DisplayMessage _displayMessage;
        private SkySpeedServices _skySpeedServices;
        private Window _parentWindow;
        private FlightDetails _selectedFlight;

        public FlightResultsPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("Flight Result");
            NumberOfPassengersLabel.Content = SharedDataPage.NumberOfPassengers;
            GetDataFromSource();
            if (SharedDataPage.FlightDetails is FlightDetails flightDetails)
            {
                FlightDetailsGrid.SelectedIndex = flightDetails.SelectedFlightIndex;
            }
        }

        private void GetDataFromSource()
        {
            try
            {
                FillFlightDetailsGrid(GetFlightDetails());
            }
            catch (Exception)
            {
                _displayMessage.ShowErrorMessageBox("Error fetching flight details. Contact support.");
            }
        }

        private List<FlightDetails> GetFlightDetails()
        {
            int flightIndex = 0;
            var flightDetailsList = new List<FlightDetails>();
            _skySpeedServices = new SkySpeedServices();

            foreach (var kvp in _skySpeedServices.GetAllFlightDetails())
            {
                var flightData = kvp.Value;

                string flightNumber = flightData.TryGetValue("FLIGHT_NUMBER", out var flightNumberObj) && flightNumberObj is string flightNumberStr ? flightNumberStr : null;
                string designator = flightData.TryGetValue("DESIGNATOR", out var designatorObj) && designatorObj is string designatorStr ? designatorStr : null;
                string dayDate = flightData.TryGetValue("DAY_DATE", out var dayDateObj) && dayDateObj is string dayDateStr ? dayDateStr : null;
                string sector = flightData.TryGetValue("SECTOR", out var sectorObj) && sectorObj is string sectorStr ? sectorStr : null;
                string departArrival = flightData.TryGetValue("DEPART_ARRIVAL", out var departArrivalObj) && departArrivalObj is string departArrivalStr ? departArrivalStr : null;
                string stop = flightData.TryGetValue("STOP", out var stopObj) && stopObj is string stopStr ? stopStr : null;
                string seatLeft = flightData.TryGetValue("SEATSLEFT", out var seatLeftObj) && seatLeftObj is string seatLeftStr ? seatLeftStr : null;
                string fare = flightData.TryGetValue("FARE", out var fareObj) && fareObj is string fareStr ? fareStr : null;
                string duration = flightData.TryGetValue("DURATION", out var durationObj) && durationObj is string durationStr ? durationStr : null;

                flightDetailsList.Add(
                    new FlightDetails(
                        flightIndex++,
                        $"..\\Images\\{designator}.png",
                        flightNumber,
                        dayDate,
                        sector,
                        departArrival,
                        stop,
                        $"+{seatLeft}",
                        $"{fare} INR",
                        duration
                    )
                );
            }
            return flightDetailsList;
        }

        private void FillFlightDetailsGrid(List<FlightDetails> flightData)
        {
            if (flightData == null || !flightData.Any())
            {
                _displayMessage.ShowWarningMessageBox("There are no flights currently available.");
                return;
            }

            foreach (var item in flightData)
            {
                FlightDetailsGrid.Items.Add(item);
            }
        }

        private void FlightDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedFlight = FlightDetailsGrid.SelectedItem as FlightDetails;
            if (_selectedFlight != null)
                CalculateTotalCost();
        }

        private void CalculateTotalCost()
        {
            if (_selectedFlight == null)
                return;

            if (double.TryParse(_selectedFlight.Fare.Replace("INR", "").Trim(), out double fareAmount))
            {
                PricePerADTLabel.Content = fareAmount;
                TotalCostLabel.Content = SharedDataPage.NumberOfPassengers == 0 ? fareAmount : fareAmount * SharedDataPage.NumberOfPassengers;
            }

            // Update the Main Parent window
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow != null)
            {
                TextBlock textBlock = (TextBlock)_parentWindow.FindName("FlightInformationExpanderTextBlock");
                textBlock.Text = $"{_selectedFlight.DayAndDate}\t{_selectedFlight.Fare}\n{_selectedFlight.FlightNumber}  {_selectedFlight.Sector}  {_selectedFlight.DepartArrival}";
            }
            SharedDataPage.FlightDetails = _selectedFlight;
        }
    }
}