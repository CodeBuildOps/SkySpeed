using SharedData;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SkySpeed.SeatMap
{
    /// <summary>
    /// Interaction logic for SeatMapPage.xaml
    /// </summary>
    public partial class SeatMapPage : Page
    {
        private Button _selectedSeat = null;
        private Image _selectedSittingPersonImage = null;

        private readonly DisplayMessage _displayMessage;
        private PassengersDetails _selectedPassenger;
        private List<SeatMapPriceDetails> _seatGroupPrice;
        private List<string> _seatLeftTop;
        private List<string> _seatRightTop;
        private List<string> _seatLeftMiddle;
        private List<string> _seatRightMiddle;
        private List<string> _seatLeftBottom;
        private List<string> _seatRightBottom;

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

            if (SharedDataPage.PassengerSeatListDetailsGrid != null)
            {
                foreach (var rowItem in SharedDataPage.PassengerSeatListDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        detailsList.Add(
                            new PassengersDetails(
                                row.PassengerId,
                                row.Type,
                                row.Gender,
                                row.Title,
                                row.FirstName,
                                row.MiddleName,
                                row.LastName,
                                row.DOB,
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
                                row.PassengerId,
                                row.Type,
                                row.Gender,
                                row.Title,
                                row.FirstName,
                                row.MiddleName,
                                row.LastName,
                                row.DOB,
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

            _seatLeftTop = new List<string>();
            _seatRightTop = new List<string>();
            _seatLeftMiddle = new List<string>();
            _seatRightMiddle = new List<string>();
            _seatLeftBottom = new List<string>();
            _seatRightBottom = new List<string>();

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
                        // Need to check
                        _seatGroupPrice.Add(new SeatMapPriceDetails(0,seatName, price));

                        // Add seats in respective seat list
                        if ( row >= 1 && row <=5 )
                        {
                            if ( col >= 'A' && col <= 'C' )
                                _seatLeftTop.Add($"{row}{col}");
                            else if ( col >= 'D' && col <= 'F')
                                _seatRightTop.Add($"{row}{col}");
                        }
                        else if ( row == 0 )
                        {
                            if ( col >= 'A' && col <= 'C' )
                                _seatLeftMiddle.Add($"{row}{col}");
                            else if ( col >= 'D' && col <= 'F' )
                                _seatRightMiddle.Add($"{row}{col}");
                        }
                        else if ( row >= 6 && row <= 9 )
                        {
                            if ( col >= 'A' && col <= 'C' )
                                _seatLeftBottom.Add($"{row}{col}");
                            else if ( col >= 'D' && col <= 'F' )
                                _seatRightBottom.Add($"{row}{col}");
                        }
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

                _selectedSeat = clickedButton;

                HighlightSelectedSeat(_selectedSeat);

                SaveSeat(_selectedSeat.Content.ToString());
            }
            else
            {
                _displayMessage.ShowErrorMessageBox("Select a passenger from the passenger seat list first.");
            }
        }

        private void DeselectPreviousSeat()
        {
            if (_selectedSeat != null)
            {
                GetSittingPersonImage(_selectedSeat);

                if (_selectedSittingPersonImage != null)
                {
                    // Get the selected button's grid position and move the sharedSittingPersonImage to the button's position
                    Grid.SetRow(_selectedSittingPersonImage, Grid.GetRow(_selectedSeat));
                    Grid.SetColumn(_selectedSittingPersonImage, Grid.GetColumn(_selectedSeat));

                    // Make the image hidden
                    _selectedSittingPersonImage.Visibility = Visibility.Hidden;
                }
            }
        }

        private void HighlightSelectedSeat(Button seat)
        {
            GetSittingPersonImage(seat);

            if (_selectedSittingPersonImage != null)
            {
                // Get the selected button's grid position and move the sharedSittingPersonImage to the button's position
                Grid.SetRow(_selectedSittingPersonImage, Grid.GetRow(seat));
                Grid.SetColumn(_selectedSittingPersonImage, Grid.GetColumn(seat));

                // Bring the image to the front
                Panel.SetZIndex(_selectedSittingPersonImage, 2);
                Panel.SetZIndex(seat, 1);

                // Make the image visible
                _selectedSittingPersonImage.Visibility = Visibility.Visible;
            }
        }

        private void GetSittingPersonImage(Button seat)
        {
            // Combined dictionary to map seat lists to their corresponding images
            var seatImageMap = new Dictionary<List<string>, Image>()
            {
                { _seatLeftTop, sittingPersonImageLeftTop },
                { _seatRightTop, sittingPersonImageRightTop },
                { _seatLeftMiddle, sittingPersonImageLeftMiddle },
                { _seatRightMiddle, sittingPersonImageRightMiddle },
                { _seatLeftBottom, sittingPersonImageLeftBottom },
                { _seatRightBottom, sittingPersonImageRightBottom }
            };

            // Reset _selectedSittingPersonImage
            _selectedSittingPersonImage = null;

            // Search in all seat lists
            foreach (var kvp in seatImageMap)
            {
                if (kvp.Key.Contains(seat.Content.ToString()))
                {
                    _selectedSittingPersonImage = kvp.Value;
                    break;
                }
            }
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