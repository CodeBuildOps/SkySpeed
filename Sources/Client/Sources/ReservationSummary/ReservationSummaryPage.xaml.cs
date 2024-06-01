using SharedData;
using SkySpeed.FlightResults;
using SkySpeed.Passengers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace SkySpeed.ReservationSummary
{
    /// <summary>
    /// Interaction logic for ReservationSummaryPage.xaml
    /// </summary>
    partial class ReservationSummaryPage : Page
    {
        private Window _parentWindow;

        public ReservationSummaryPage()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _parentWindow = Window.GetWindow(this);
            if (_parentWindow != null)
            {
                InitializeData();
            }
        }

        private void InitializeData()
        {
            GetDetailsFromMainParentWindow("FlightInformationExpanderTextBlock", FlightDetailsTextBlock);
            GetDetailsFromMainParentWindow("PassengersExpanderTextBlock", PassengersTextBlock);
            GetDetailsFromMainParentWindow("ContactExpanderTextBlock", ContactInformationTextBlock);
            GetDetailsFromMainParentWindow("CommentTextBox", CommentTextBlock);
            SetCostSummary();

            // SetPaymentInformation();
        }

        private void GetDetailsFromMainParentWindow(string parentElementName, FrameworkElement updateElement)
        {
            if (_parentWindow == null) return;

            var parentElement = _parentWindow.FindName(parentElementName) as FrameworkElement;

            string textContent = null;

            if (parentElement is TextBlock parentTextBlock)
            {
                textContent = parentTextBlock.Text;
            }
            else if (parentElement is TextBox parentTextBox)
            {
                textContent = parentTextBox.Text;
            }

            if (!string.IsNullOrEmpty(textContent))
            {
                if (updateElement is TextBlock updateTextBlock)
                {
                    updateTextBlock.Text = textContent;
                }
                else if (updateElement is TextBox updateTextBox)
                {
                    updateTextBox.Text = textContent;
                }
            }
        }

        private void SetCostSummary()
        {
            double flightCost = 0;
            double totalSeatCost = 0;
            double totalCost = 0;

            // Check if FlightDetails is available
            if (SharedDataPage.FlightDetails is FlightDetails flightDetails)
            {
                if (double.TryParse(flightDetails.Fare.Replace("INR", "").Trim(), out flightCost))
                {
                    FlightPriceLabel.Content = $"{flightCost:F} INR";
                }
                else
                {
                    FlightPriceLabel.Content = "N/A";
                }
            }
            else
            {
                FlightPriceLabel.Content = "N/A";
            }

            // Check if PassengersDetailsGrid is available
            if (SharedDataPage.PassengersDetailsGrid != null)
            {
                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        totalSeatCost += row.SeatPrice;
                    }
                }
                SeatPriceLabel.Content = $"{totalSeatCost:F} INR";
            }
            else
            {
                SeatPriceLabel.Content = "N/A";
            }

            totalCost = flightCost + totalSeatCost;
            TotalCostLabel.Content = $"{totalCost:F} INR";
        }
    }
}