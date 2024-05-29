using Constant;
using SharedData;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SkySpeed.Contacts
{
    /// <summary>
    /// Interaction logic for ContactsPage.xaml
    /// </summary>
    public partial class ContactsPage : Page
    {
        private readonly DisplayMessage _displayMessage;
        private PassengersDetails _selectedPassenger;
        private Window _parentWindow;

        public ContactsPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("Contact");

            if (SharedDataPage.PassengersDetailsGrid != null)
                SetContactDetailsGrid();

            SetComboBoxWithCountries();

            // Disable group boxes and buttons initially
            EditContactButton.IsEnabled = false;
            ContactDetailsGroupBox.IsEnabled = false;
            SaveButton.IsEnabled = false;
            LastNameTextBox.IsEnabled = false;
            FirstNameTextBox.IsEnabled = false;
            MiddleNameTextBox.IsEnabled = false;
            TitleComboBox.IsEnabled = false;
        }

        private void SetComboBoxWithCountries()
        {
            var countries = Enum.GetNames(typeof(ConstantHandler.Countries));
            CountryComboBox.ItemsSource = countries;
        }

        private void SetContactDetailsGrid()
        {
            var passengerDetailsList = new List<PassengersDetails>();
            if (SharedDataPage.PassengersDetailsGrid != null)
            {
                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        passengerDetailsList.Add(
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
                FillContactDetailsGrid(passengerDetailsList);
            }
        }

        private void FillContactDetailsGrid(List<PassengersDetails> passengerData)
        {
            foreach (var item in passengerData)
            {
                ContactDetailsGrid.Items.Add(item);
            }
        }

        private void ContactDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPassenger = ContactDetailsGrid.SelectedItem as PassengersDetails;
            EditContactButton.IsEnabled = _selectedPassenger != null;
        }

        private void EditContactButton_Click(object sender, RoutedEventArgs e)
        {
            SaveButton.IsEnabled = true;
            ContactDetailsGroupBox.IsEnabled = true;
            PopulateContactDetailsInFields(_selectedPassenger);
        }

        private void PopulateContactDetailsInFields(PassengersDetails selectedPassenger)
        {
            LastNameTextBox.Text = selectedPassenger.LastName;
            FirstNameTextBox.Text = selectedPassenger.FirstName;
            MiddleNameTextBox.Text = selectedPassenger.MiddleName;
            TitleComboBox.Text = selectedPassenger.Title;

            Line1TextBox.Text = selectedPassenger.AddressLine1;
            Line2TextBox.Text = selectedPassenger.AddressLine2;
            PostalTextBox.Text = selectedPassenger.AddressPostal;
            TownTextBox.Text = selectedPassenger.AddressTown;
            AddressStateTextBox.Text = selectedPassenger.AddressState;
            CountryComboBox.Text = selectedPassenger.AddressCountry;
            MobileTextBox.Text = selectedPassenger.Mobile;
            EmailTextBox.Text = selectedPassenger.Email;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidatePassengerFields(Line1TextBox.Text, Line2TextBox.Text, PostalTextBox.Text, TownTextBox.Text, AddressStateTextBox.Text, CountryComboBox.Text,
            MobileTextBox.Text, EmailTextBox.Text))
                return;

            SaveContact(EmailTextBox.Text, MobileTextBox.Text, Line1TextBox.Text, Line2TextBox.Text, PostalTextBox.Text, TownTextBox.Text, AddressStateTextBox.Text, CountryComboBox.Text);

            ClearAllFields();

            SharedDataPage.ContactDetailsGrid = ContactDetailsGrid;
            SharedDataPage.PassengersDetailsGrid = ContactDetailsGrid;
            UpdateMainParentWindow();
        }

        private bool ValidatePassengerFields(string addressLine1, string addressLine2, string addressPostal, string addressTown, string addressState, string addressCountry,
            string mobile, string email)
        {
            if (string.IsNullOrEmpty(addressLine1) || string.IsNullOrEmpty(addressLine2))
            {
                _displayMessage.ShowWarningMessageBox("Address line must not remain empty.");
                return false;
            }

            // Postal is optional
            //else if (string.IsNullOrEmpty(addressPostal))
            //{
            //    _displayMessage.ShowWarningMessageBox("Address postal must not remain empty.");
            //    return false;
            //}

            // Town is optional
            //else if (string.IsNullOrEmpty(addressTown))
            //{
            //    _displayMessage.ShowWarningMessageBox("Address town not remain empty.");
            //    return false;
            //}

            // State is optional
            //else if (string.IsNullOrEmpty(addressState))
            //{
            //    _displayMessage.ShowWarningMessageBox("Address state must not remain empty.");
            //    return false;
            //}

            // Country is optional
            //else if (string.IsNullOrEmpty(addressCountry))
            //{
            //    _displayMessage.ShowWarningMessageBox("Address country must not remain empty.");
            //    return false;
            //}

            // Mobile is optional
            //else if (string.IsNullOrEmpty(mobile))
            //{
            //    _displayMessage.ShowWarningMessageBox("Mobile must not remain empty.");
            //    return false;
            //}
            else if (string.IsNullOrEmpty(email))
            {
                _displayMessage.ShowWarningMessageBox("Email must not remain empty.");
                return false;
            }
            return true;
        }

        private void SaveContact(string email, string mobile, string addressLine1, string addressLine2, string addressPostal, string addressTown, string addressState, string addressCountry)
        {
            var selectedPassengerInContactDetailGrid = ContactDetailsGrid.Items[ContactDetailsGrid.SelectedIndex] as PassengersDetails;
            selectedPassengerInContactDetailGrid.AddressLine1 = addressLine1;
            selectedPassengerInContactDetailGrid.AddressLine2 = addressLine2;
            selectedPassengerInContactDetailGrid.AddressPostal = addressPostal;
            selectedPassengerInContactDetailGrid.AddressTown = addressTown;
            selectedPassengerInContactDetailGrid.AddressState = addressState;
            selectedPassengerInContactDetailGrid.AddressCountry = addressCountry;
            selectedPassengerInContactDetailGrid.FullAddress = $"{addressLine1}, {addressLine2}, {addressPostal}, {addressTown}, {addressState}, {addressCountry}";
            selectedPassengerInContactDetailGrid.Mobile = mobile;
            selectedPassengerInContactDetailGrid.Email = email;

            ContactDetailsGrid.Items.Refresh();

            // Deselect the selected passenger
            ContactDetailsGrid.SelectedItem = null;
        }

        private void ClearAllFields()
        {
            ClearContactFields(LastNameTextBox, FirstNameTextBox, MiddleNameTextBox, TitleComboBox,
                Line1TextBox, Line2TextBox, PostalTextBox, TownTextBox, AddressStateTextBox, CountryComboBox, MobileTextBox, EmailTextBox);

            ContactDetailsGroupBox.IsEnabled = false;
            SaveButton.IsEnabled = false;
            EditContactButton.IsEnabled = false;
        }

        private void ClearContactFields(params Control[] controls)
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
            }
        }

        private void UpdateMainParentWindow()
        {
            if (SharedDataPage.ContactDetailsGrid != null)
            {
                _parentWindow = Window.GetWindow(this);
                TextBlock textBlock = _parentWindow.FindName("ContactExpanderTextBlock") as TextBlock;

                // Clear the existing text
                textBlock.Text = string.Empty;

                StringBuilder textBuilder = new StringBuilder();
                foreach (var rowItem in SharedDataPage.ContactDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        textBuilder.AppendLine($"Passenger - {row.PassengerId}");
                        textBuilder.AppendLine($"{row.Mobile}");
                        textBuilder.AppendLine($"{row.Email}");
                        textBuilder.AppendLine($"{row.FullAddress}");
                        textBuilder.AppendLine();
                    }
                }

                // Update the TextBlock with the new text
                textBlock.Text = textBuilder.ToString();
            }
        }
    }
}