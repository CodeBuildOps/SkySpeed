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
        private DisplayMessage _displayMessage;
        private SkySpeedServices _skySpeedServices;
        private Window _parentWindow;

        public FlightResultsPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("Flight Result");
            _skySpeedServices = new SkySpeedServices();

            NumberOfPassengersLabel.Content = SharedDataPage.NumberOfPassengers;
            GetDataFromSource();
        }

        private void GetDataFromSource()
        {
            try
            {
                FillFlightDetailsGrid(GetFlightDetails());
            }
            catch (Exception)
            {
                _displayMessage.ShowErrorMessageBox("Error fetching flight details.");
            }
        }

        private List<FlightDetails> GetFlightDetails()
        {
            var flightDetailsList = new List<FlightDetails>();
            string flightNumber = null;
            string designator = null;
            string dayDate = null;
            string sector = null;
            string departArrival = null;
            string stop = null;
            string seatLeft = null;
            string fare = null;
            string duration = null;

            foreach (var kvp in _skySpeedServices.GetAllFlightDetails())
            {
                if (kvp.Value.TryGetValue("FLIGHT_NUMBER", out object flightNumberObject))
                {
                    if (flightNumberObject != null && flightNumberObject is string flightNumberString)
                    {
                        flightNumber = flightNumberString;
                    }
                }
                if (kvp.Value.TryGetValue("DESIGNATOR", out object designatorObject))
                {
                    if (designatorObject != null && designatorObject is string designatorString)
                    {
                        designator = designatorString;
                    }
                }
                if (kvp.Value.TryGetValue("DAY_DATE", out object dayDateObject))
                {
                    if (dayDateObject != null && dayDateObject is string dayDateString)
                    {
                        dayDate = dayDateString;
                    }
                }
                if (kvp.Value.TryGetValue("SECTOR", out object sectorObject))
                {
                    if (sectorObject != null && sectorObject is string sectorString)
                    {
                        sector = sectorString;
                    }
                }
                if (kvp.Value.TryGetValue("DEPART_ARRIVAL", out object departArrivalObject))
                {
                    if (departArrivalObject != null && departArrivalObject is string departArrivalString)
                    {
                        departArrival = departArrivalString;
                    }
                }
                if (kvp.Value.TryGetValue("STOP", out object stopObject))
                {
                    if (stopObject != null && stopObject is string stopString)
                    {
                        stop = stopString;
                    }
                }
                if (kvp.Value.TryGetValue("SEATSLEFT", out object seatLeftObject))
                {
                    if (seatLeftObject != null && seatLeftObject is string seatLeftString)
                    {
                        seatLeft = seatLeftString;
                    }
                }
                if (kvp.Value.TryGetValue("FARE", out object fareObject))
                {
                    if (fareObject != null && fareObject is string fareString)
                    {
                        fare = fareString;
                    }
                }
                if (kvp.Value.TryGetValue("DURATION", out object durationObject))
                {
                    if (durationObject != null && durationObject is string durationString)
                    {
                        duration = durationString;
                    }
                }

                flightDetailsList.Add(
                    new FlightDetails (
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
            if (flightData != null && flightData.Any())
            {
                foreach (var item in flightData)
                {
                    FlightDetailsGrid.Items.Add(item);
                }
            }
            else
            {
                _displayMessage.ShowWarningMessageBox("There are no flights currently available.");
            }
        }

        private void FlightDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateTotalCost(FlightDetailsGrid.SelectedItem as FlightDetails);
        }

        private void CalculateTotalCost(FlightDetails selectedFlight)
        {
            if (selectedFlight != null)
            {
                double fareAmount;
                if (double.TryParse(selectedFlight.Fare.Replace("INR", ""), out fareAmount))
                {
                    PricePerADTLabel.Content = fareAmount;
                    TotalCostLabel.Content = SharedDataPage.NumberOfPassengers == 0 ? fareAmount : fareAmount * SharedDataPage.NumberOfPassengers;
                }

                // Update the Main Parent window
                _parentWindow = Window.GetWindow(this);
                TextBlock textBlock = (TextBlock)_parentWindow.FindName("FlightInformationExpanderTextBlock");
                textBlock.Text =  $"{selectedFlight.DayAndDate}\t{selectedFlight.Fare}\n{selectedFlight.FlightNumber}  {selectedFlight.Sector}  {selectedFlight.DepartArrival}";
            }
        }
    }
}