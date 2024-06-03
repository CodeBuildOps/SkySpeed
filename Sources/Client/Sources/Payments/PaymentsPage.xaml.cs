using SharedData;
using SkySpeed.FlightResults;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SkySpeed.Payments
{
    /// <summary>
    /// Interaction logic for PaymentsPage.xaml
    /// </summary>
    public partial class PaymentsPage : Page
    {
        private readonly DisplayMessage _displayMessage;
        private PassengersDetails _selectedPassenger;
        private Window _parentWindow;

        public PaymentsPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("Passenger");

            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
            {
                SetCostSummary();
                SetPaymentDetailsGrid();
            }

            // Disable group boxes and buttons initially
            AmountTextBox.IsEnabled = false;
            PaymentDetailsGroupBox.IsEnabled = false;
            EditPaymentButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
        }

        private void SetCostSummary()
        {
            double totalSeatCost = 0;

            // Check if FlightDetails is available and parse the flight cost
            if (SharedDataPage.FlightDetails is FlightDetails flightDetails && double.TryParse(flightDetails.Fare.Replace("INR", "").Trim(), out double flightCost))
            {
                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        totalSeatCost += row.SeatPrice;
                    }
                }
            }
            else
            {
                AmountTextBox.Text = "N/A";
                return;
            }

            double totalCost = flightCost + totalSeatCost;
            AmountTextBox.Text = $"{totalCost:F} INR";
        }

        private void SetPaymentDetailsGrid()
        {
            var passengerUpdatedDetailsList = new List<PassengersDetails>();

            foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
            {
                if (rowItem is PassengersDetails row && row.PaymentDetailsObject != null)
                {
                    passengerUpdatedDetailsList.Add(new PassengersDetails(
                        row.PaymentDetailsObject.PaymentMethod,
                        row.PaymentDetailsObject.CardNumber,
                        row.PaymentDetailsObject.Amount,
                        row.PaymentDetailsObject.ExpirationMonth,
                        row.PaymentDetailsObject.ExpirationYear,
                        row.PaymentDetailsObject.CardHolderName
                    ));

                    // Payment details are the same for all passengers, break after first valid entry
                    break;
                }
            }

            FillDetailsGrid(passengerUpdatedDetailsList);
        }

        private void FillDetailsGrid(List<PassengersDetails> passengerData)
        {
            foreach (var item in passengerData)
            {
                PaymentDetailsGrid.Items.Add(item);
            }
        }

        private void AddPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if there is a selected payment and deselect if necessary
            if (_selectedPassenger != null)
            {
                // Deselect the selected payment
                PaymentDetailsGrid.SelectedItem = null;
            }

            Add_Edit_Payment.Content = "Add Payment";

            // Enable Payment Details Group Box if AmountTextBox.Text != "N/A"
            PaymentDetailsGroupBox.IsEnabled = true;

            // Enable Save Button if Payment Group Box is enabled
            SaveButton.IsEnabled = PaymentDetailsGroupBox.IsEnabled;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields((PaymentMethodComboBox.SelectedItem as ComboBoxItem).Content?.ToString(), CardNumberTextBox.Text, ExpiryMonthTextBox.Text, ExpiryYearTextBox.Text, CardHolderNameTextBox.Text))
                return;

            SaveOrUpdate((PaymentMethodComboBox.SelectedItem as ComboBoxItem).Content?.ToString(), CardNumberTextBox.Text, AmountTextBox.Text, ExpiryMonthTextBox.Text, ExpiryYearTextBox.Text, CardHolderNameTextBox.Text);

            ClearAllFields(CardNumberTextBox, AmountTextBox, ExpiryMonthTextBox, ExpiryYearTextBox, CardHolderNameTextBox);

            PaymentDetailsGroupBox.IsEnabled = false;
            SaveButton.IsEnabled = false;
            EditPaymentButton.IsEnabled = false;

            UpdateSharedDataPage();
            UpdateMainParentWindow();
        }

        private bool ValidateFields(string paymentMethod, string cardNumber, string expiryMonthTextBox, string expiryYearTextBox, string cardHolderName)
        {
            if (string.IsNullOrEmpty(paymentMethod))
            {
                _displayMessage.ShowWarningMessageBox("Payment method must not remain empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(cardNumber))
            {
                _displayMessage.ShowWarningMessageBox("Card number must not remain empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(expiryMonthTextBox))
            {
                _displayMessage.ShowWarningMessageBox("Expiry month must not remain empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(expiryYearTextBox))
            {
                _displayMessage.ShowWarningMessageBox("Expiry year must not remain empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(cardHolderName))
            {
                _displayMessage.ShowWarningMessageBox("Card holder name must not remain empty.");
                return false;
            }
            return true;
        }

        private void SaveOrUpdate(string paymentMethod, string cardNumber, string amount, string expiryMonth, string expiryYear, string cardHolderName)
        {
            // Update the selected payment's data
            if (_selectedPassenger != null)
            {
                var selectedPassengerInPaymentDetailGrid = PaymentDetailsGrid.Items[PaymentDetailsGrid.SelectedIndex] as PassengersDetails;
                selectedPassengerInPaymentDetailGrid.PaymentDetailsObject.PaymentMethod = paymentMethod;
                selectedPassengerInPaymentDetailGrid.PaymentDetailsObject.CardNumber = cardNumber;
                selectedPassengerInPaymentDetailGrid.PaymentDetailsObject.Amount = amount;
                selectedPassengerInPaymentDetailGrid.PaymentDetailsObject.ExpirationMonth = expiryMonth;
                selectedPassengerInPaymentDetailGrid.PaymentDetailsObject.ExpirationYear = expiryYear;
                selectedPassengerInPaymentDetailGrid.PaymentDetailsObject.CardHolderName = cardHolderName;

                PaymentDetailsGrid.Items.Refresh();

                // Deselect the selected payment
                PaymentDetailsGrid.SelectedItem = null;
            }
            // Add a new payment
            else
            {
                FillDetailsGrid(new List<PassengersDetails>() {
                    new PassengersDetails (
                        paymentMethod,
                        cardNumber,
                        amount,
                        expiryMonth,
                        expiryYear,
                        cardHolderName
                     )
                });
            }
        }

        private void UpdateMainParentWindow()
        {
            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
            {
                _parentWindow = Window.GetWindow(this);
                if (_parentWindow != null)
                {
                    if (_parentWindow.FindName("PaymentExpanderTextBlock") is TextBlock textBlock)
                    {
                        // Clear the existing text
                        textBlock.Text = string.Empty;

                        StringBuilder textBuilder = new StringBuilder();
                        foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                        {
                            if (rowItem is PassengersDetails row && row.PaymentDetailsObject != null)
                            {
                                textBuilder.AppendLine($"{row.PaymentDetailsObject.PaymentMethod}");
                                textBuilder.AppendLine($"CardNumber - {row.PaymentDetailsObject.CardNumber}");
                                textBuilder.AppendLine($"{row.PaymentDetailsObject.ExpirationMonth}/{row.PaymentDetailsObject.ExpirationYear} {row.PaymentDetailsObject.CardHolderName}");
                                break;
                            }
                        }

                        // Update the TextBlock with the new text
                        textBlock.Text = textBuilder.ToString();
                    }
                }
            }
        }

        private void UpdateSharedDataPage()
        {
            if (PaymentDetailsGrid?.Items != null)
            {
                var paymentDetailsGridItems = PaymentDetailsGrid.Items.Cast<PassengersDetails>().ToList();
                var paymentDetails = paymentDetailsGridItems[0].PaymentDetailsObject;

                var paymentDetailObject = new PaymentDetails(
                    paymentDetails.PaymentMethod,
                    paymentDetails.CardNumber,
                    paymentDetails.Amount,
                    paymentDetails.ExpirationMonth,
                    paymentDetails.ExpirationYear,
                    paymentDetails.CardHolderName
                );

                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        row.PaymentDetailsObject = paymentDetailObject;
                    }
                }
            }
        }

        private void ClearAllFields(params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Clear();
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectedItem = null;
                }
                else if (control is DatePicker datePicker)
                {
                    datePicker.SelectedDate = null;
                }
            }
        }

        private void PaymentDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPassenger = PaymentDetailsGrid.SelectedItem as PassengersDetails;
            EditPaymentButton.IsEnabled = _selectedPassenger != null;
        }

        private void EditPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            Add_Edit_Payment.Content = "Edit Payment";
            SaveButton.IsEnabled = true;
            PaymentDetailsGroupBox.IsEnabled = true;
            PopulateDetails();
        }

        private void PopulateDetails()
        {
            PaymentMethodComboBox.Text = _selectedPassenger.PaymentDetailsObject.PaymentMethod;
            CardNumberTextBox.Text = _selectedPassenger.PaymentDetailsObject.CardNumber;
            AmountTextBox.Text = _selectedPassenger.PaymentDetailsObject.Amount;
            ExpiryMonthTextBox.Text = _selectedPassenger.PaymentDetailsObject.ExpirationMonth;
            ExpiryYearTextBox.Text = _selectedPassenger.PaymentDetailsObject.ExpirationYear;
            CardHolderNameTextBox.Text = _selectedPassenger.PaymentDetailsObject.CardHolderName;
        }
    }
}