using SharedData;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SkySpeed.SeatMap
{
    /// <summary>
    /// Interaction logic for SeatMapPage.xaml
    /// </summary>
    partial class SeatMapPage : Page
    {
        private Button _selectedSeat = null;
        private Image _selectedSittingPersonImage = null;
        private Window _parentWindow;
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

            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
            {
                InitializeSeats();
                SetPassengerSeatListDetailsGrid();

                // Todo: Populate seat availability
            }
            else
            {
                PassengerSeatListDetailsGrid.IsEnabled = false;
                _displayMessage.ShowErrorMessageBox("No passengers to select. Please add a passenger.");
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

                    // Get the Button object using FindName
                    Button seatButton = (Button)FindName(seatName);

                    // Check if seatButton is found
                    if (seatButton != null)
                    {
                        seatButton.Click += ButtonSeat_Clicked;
                        seatButton.MouseEnter += ButtonSeat_MouseEnter;
                        seatButton.MouseLeave += ButtonSeat_MouseLeave;

                        // Calculate seat price based on location
                        double price = CalculateSeatPrice(row, col);

                        // Add seat price details to the list
                        _seatGroupPrice.Add(new SeatMapPriceDetails(seatName, price));

                        // Add seats in respective seat list
                        AddSeatToGroupList(row, col);
                    }
                }
            }

            // Add seat details to the grid
            foreach (SeatMapPriceDetails item in _seatGroupPrice)
            {
                SeatGroupDetailsGrid.Items.Add(item);
            }
        }

        private void ButtonSeat_MouseEnter(object sender, MouseEventArgs e)
        {
            Button hoverSeat = (Button)sender;
            SeatMapPriceDetails seatGroupAndPrice = GetSeatMapPriceDetails(hoverSeat.Content.ToString());
            if (seatGroupAndPrice != null)
            {
                string seatPriceText = $"{seatGroupAndPrice.SeatPrice} INR";
                Brush backgroundColor;

                if (seatGroupAndPrice.SeatPrice > 500)
                {
                    backgroundColor = Brushes.Red;
                }
                else
                {
                    backgroundColor = seatGroupAndPrice.SeatPrice > 0 && seatGroupAndPrice.SeatPrice <= 500 ? Brushes.Yellow : (Brush)Brushes.Green;
                }

                ToolTip tooltip = new ToolTip
                {
                    Content = new TextBlock
                    {
                        Text = seatPriceText,
                        Padding = new Thickness(5),
                        Foreground = Brushes.Black,
                        FontWeight = FontWeights.Bold,
                        FontSize = 14,
                    },
                    Background = backgroundColor
                };

                hoverSeat.ToolTip = tooltip;
            }
        }

        private void ButtonSeat_MouseLeave(object sender, MouseEventArgs e)
        {
            Button hoverSeat = (Button)sender;
            hoverSeat.ToolTip = null;
        }

        private void AddSeatToGroupList(int row, char col)
        {
            if (row >= 1 && row <= 5)
            {
                if (col >= 'A' && col <= 'C')
                {
                    _seatLeftTop.Add($"{row}{col}");
                }
                else if (col >= 'D' && col <= 'F')
                {
                    _seatRightTop.Add($"{row}{col}");
                }
            }
            else if (row == 0)
            {
                if (col >= 'A' && col <= 'C')
                {
                    _seatLeftMiddle.Add($"{row}{col}");
                }
                else if (col >= 'D' && col <= 'F')
                {
                    _seatRightMiddle.Add($"{row}{col}");
                }
            }
            else if (row >= 6 && row <= 9)
            {
                if (col >= 'A' && col <= 'C')
                {
                    _seatLeftBottom.Add($"{row}{col}");
                }
                else if (col >= 'D' && col <= 'F')
                {
                    _seatRightBottom.Add($"{row}{col}");
                }
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
            else
            {
                return col == 'A' || col == 'F' ? 500 : 0;
            }
        }

        private void SetPassengerSeatListDetailsGrid()
        {
            List<PassengersDetails> detailsList = new List<PassengersDetails>();

            foreach (object rowItem in SharedDataPage.PassengersDetailsGrid.Items)
            {
                if (rowItem is PassengersDetails row)
                {
                    detailsList.Add(new PassengersDetails(
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
                        row.SeatPrice,

                        row.PaymentDetailsObject
                        ));
                }
            }

            FillPassengerSeatListDetailsGrid(detailsList);
        }

        private void FillPassengerSeatListDetailsGrid(List<PassengersDetails> passengerData)
        {
            foreach (PassengersDetails item in passengerData)
            {
                if (item.Seat != null)
                {
                    // Preserve the spot where the seating person image will be set for each passenger.
                    // Locate the button
                    Button seatButton = (Button)FindName($"Seat{item.Seat}");
                    GetSittingPersonImage(seatButton);
                    SetSittingPersonImage(seatButton);
                }
                PassengerSeatListDetailsGrid.Items.Add(item);
            }
        }

        private void PassengerSeatListDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPassenger = PassengerSeatListDetailsGrid.SelectedItem as PassengersDetails;
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
                SetSittingPersonImage(_selectedSeat);
            }
        }

        private void HighlightSelectedSeat(Button seat)
        {
            GetSittingPersonImage(seat);
            SetSittingPersonImage(seat);

        }

        private void GetSittingPersonImage(Button seat)
        {
            // Combined dictionary to map seat lists to their corresponding images
            Dictionary<List<string>, Image> seatImageMap = new Dictionary<List<string>, Image>()
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
            foreach (KeyValuePair<List<string>, Image> kvp in seatImageMap)
            {
                if (kvp.Key.Contains(seat.Content.ToString()))
                {
                    _selectedSittingPersonImage = kvp.Value;
                    break;
                }
            }
        }

        private void SetSittingPersonImage(Button seat)
        {
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

        private void SaveSeat(string selectedSeat)
        {
            SeatMapPriceDetails seatGroupAndPrice = GetSeatMapPriceDetails(selectedSeat);
            if (seatGroupAndPrice != null)
            {
                _selectedPassenger.Seat = selectedSeat;
                _selectedPassenger.SeatPrice = seatGroupAndPrice.SeatPrice;
            }

            // Refresh the DataGrid to reflect the changes
            PassengerSeatListDetailsGrid.Items.Refresh();

            // Deselect the selected passenger
            PassengerSeatListDetailsGrid.SelectedItem = null;

            SharedDataPage.PassengersDetailsGrid = PassengerSeatListDetailsGrid;
            UpdateMainParentWindow();
        }

        private SeatMapPriceDetails GetSeatMapPriceDetails(string selectedSeat)
        {
            return SeatGroupDetailsGrid.Items.Cast<SeatMapPriceDetails>().FirstOrDefault(x => x.Seat == $"Seat{selectedSeat}");
        }

        private void UpdateMainParentWindow()
        {
            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
            {
                _parentWindow = Window.GetWindow(this);
                TextBlock textBlock = _parentWindow.FindName("SeatExpanderTextBlock") as TextBlock;

                // Clear the existing text
                textBlock.Text = string.Empty;

                StringBuilder textBuilder = new StringBuilder();
                foreach (object rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        textBuilder.AppendLine($"Passenger - {row.PassengerId}");
                        textBuilder.AppendLine($"Seat: {row.Seat}\t{row.SeatPrice}");
                        textBuilder.AppendLine();
                    }
                }

                // Update the TextBlock with the new text
                textBlock.Text = textBuilder.ToString();
            }
        }
    }
}