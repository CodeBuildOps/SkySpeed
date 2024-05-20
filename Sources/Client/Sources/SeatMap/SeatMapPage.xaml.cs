using SharedData;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SkySpeed.SeatMap
{
    /// <summary>
    /// Interaction logic for SeatMapPage.xaml
    /// </summary>
    public partial class SeatMapPage : Page
    {
        private Button selectedSeat = null;

        private DisplayMessage _displayMessage;
        private PassengersDetails _selectedPassenger;
        private List<SeatMapPriceDetails> _seatGroupPrice;

        public SeatMapPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("Seat Map");

            if (SharedDataPage.PassengersDetailsGrid != null)
            {
                SetPassengerSeatListDetailsGrid();
                InitializeSeats();
            }
            else
                PassengerSeatListDetailsGrid.IsEnabled = false;
        }

        private void SetPassengerSeatListDetailsGrid()
        {
            var passengersDetailsList = new List<PassengersDetails>();

            foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
            {
                if (rowItem is PassengersDetails row)
                {
                    passengersDetailsList.Add(
                        new PassengersDetails(
                            row.Type,
                            row.Gender,
                            row.Title,
                            row.FirstName,
                            row.MiddleName,
                            row.LastName,
                            row.DOB,
                            row.Nationality,
                            row.Country,

                            row.AddressLine1,
                            row.AddressLine2,
                            row.AddressPostal,
                            row.AddressTown,
                            row.AddressState,
                            row.AddressCountry,
                            row.Mobile,
                            row.Email,

                            row.Seat
                            )
                    );
                }
            }

            FillPassengerSeatListDetailsGrid(passengersDetailsList);
        }

        private void FillPassengerSeatListDetailsGrid(List<PassengersDetails> passengerData)
        {
            foreach (var item in passengerData)
            {
                PassengerSeatListDetailsGrid.Items.Add(item);
            }
        }

        private void InitializeSeats()
        {
            _seatGroupPrice = new List<SeatMapPriceDetails>();

            // Define seat range
            int startRow = 0, endRow = 9;
            char startColumn = 'A', endColumn = 'F';

            // Loop through rows and columns
            for (int row = startRow; row <= endRow; row++)
            {
                for (char col = startColumn; col <= endColumn; col++)
                {
                    string seatName = $"Seat{row}{col}";

                    // Get the ToggleButton object using FindName
                    Button seatButton = (Button)this.FindName(seatName);

                    // Check if seatButton is found
                    if (seatButton != null)
                    {
                        seatButton.Click += ButtonSeat_Clicked;

                        // Calculate seat price based on location
                        double price = CalculateSeatPrice(row, col);

                        // Add seat details to the list
                        _seatGroupPrice.Add(new SeatMapPriceDetails(seatName, price));
                    }
                }
            }

            // Add seat details to the grid
            foreach (var item in _seatGroupPrice)
            {
                SeatGroupDetailsGrid.Items.Add(item);
            }
        }

        private double CalculateSeatPrice(int row, char col)
        {
            // Business seats
            if (row == 1)
            {
                return 1000;
            }
            // Window seats
            else if (col == 'A' || col == 'F')
            {
                return 500;
            }
            // Other seats
            else
            {
                return 0;
            }
        }

        private void PassengerSeatListDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPassenger = PassengerSeatListDetailsGrid.SelectedItem as PassengersDetails;

            // Todo: Get some validation like seat available, or enable seat selection
        }

        private void ButtonSeat_Clicked(object sender, RoutedEventArgs e)
        {
            if (_selectedPassenger != null)
            {
                Button clickedButton = (Button)sender;

                // Deselect the previously selected seat, if any
                DeselectPreviousSeat();

                // Update the selected seat
                selectedSeat = clickedButton;

                // Highlight the selected seat
                HighlightSelectedSeat(selectedSeat);

                // Todo: Get the selected seat price and handle further actions
                SaveSeat(selectedSeat.Content.ToString(), 100);
            }
            else
            {
                _displayMessage.ShowWarningMessageBox("Select passenger first.");
            }
        }

        private void DeselectPreviousSeat()
        {
            if (selectedSeat != null)
            {
                // Revert back the appearance of the previously selected seat
                selectedSeat.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6699CC"));
            }
        }

        private void HighlightSelectedSeat(Button seat)
        {
            // Set the appearance of the selected seat
            seat.Background = Brushes.Red;
        }

        private void SaveSeat(string selectedSeat, double selectedSeatPrice)
        {
            if (_selectedPassenger != null)
            {
                // Update the selected passenger's seat
                _selectedPassenger.Seat = selectedSeat;
                _selectedPassenger.SeatPrice = selectedSeatPrice;

                // Refresh the DataGrid to reflect the changes
                PassengerSeatListDetailsGrid.Items.Refresh();

                // Need to work here
                //SharedDataPage.PassengersDetailsGrid = PassengerSeatListDetailsGrid;

                // Deselect the selected passenger
                PassengerSeatListDetailsGrid.SelectedItem = null;

                // Update the Main Parent window
                //_parentWindow = Window.GetWindow(this);
                //TextBlock textBlock = (TextBlock)_parentWindow.FindName("PassengersExpanderTextBlock");
                //textBlock.Text += $"\nSeat: {selectedSeat}";
            }
        }
    }
}