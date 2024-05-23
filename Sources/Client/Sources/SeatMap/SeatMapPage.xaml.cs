using SharedData;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using System.Collections.Generic;
using System.Linq;
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
            var detailsList = new List<PassengersDetails>();

            var gridItems = SharedDataPage.PassengerSeatListDetailsGrid?.Items ?? SharedDataPage.PassengersDetailsGrid.Items;

            if (SharedDataPage.PassengerSeatListDetailsGrid != null)
            {
                foreach (var rowItem in SharedDataPage.PassengerSeatListDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        detailsList.Add(
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

                                row.Seat,
                                row.SeatPrice
                                )
                        );
                    }
                }
            }
            else
            {
                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        detailsList.Add(
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

                                row.Seat,
                                row.SeatPrice
                                )
                        );
                    }
                }
            }

            FillPassengerSeatListDetailsGrid(detailsList);
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

                DeselectPreviousSeat();

                selectedSeat = clickedButton;

                HighlightSelectedSeat(selectedSeat);

                SaveSeat(selectedSeat.Content.ToString());
            }
            else
            {
                _displayMessage.ShowErrorMessageBox("Select a passenger from the passenger seat list first.");
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
            seat.Background = Brushes.Green;
        }

        private void SaveSeat(string selectedSeat)
        {
            if (_selectedPassenger != null)
            {
                var seatGroupAndPrice = SeatGroupDetailsGrid.Items.Cast<SeatMapPriceDetails>().FirstOrDefault(x => x.Seat == $"Seat{selectedSeat}");
                if (seatGroupAndPrice != null)
                {
                    _selectedPassenger.Seat = selectedSeat;
                    _selectedPassenger.SeatPrice = seatGroupAndPrice.SeatPrice;
                }

                _displayMessage.ShowSuccessMessageBox($"" +
                    $"Seat: {_selectedPassenger.Seat}\n" +
                    $"SeatPrice: {_selectedPassenger.SeatPrice}\n" +
                    $"Passenger: {_selectedPassenger.FullName}");

                // Refresh the DataGrid to reflect the changes
                PassengerSeatListDetailsGrid.Items.Refresh();

                // Deselect the selected passenger
                PassengerSeatListDetailsGrid.SelectedItem = null;

                // Todo: Update the Main Parent window like in ExpanderTextBlock
                //_parentWindow = Window.GetWindow(this);
                //TextBlock textBlock = (TextBlock)_parentWindow.FindName("PassengersExpanderTextBlock");
                //textBlock.Text += $"\nSeat: {selectedSeat}";

                SharedDataPage.PassengerSeatListDetailsGrid = PassengerSeatListDetailsGrid;
            }
        }
    }
}