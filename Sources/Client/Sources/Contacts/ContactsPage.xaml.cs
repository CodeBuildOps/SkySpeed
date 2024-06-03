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
    partial class ContactsPage : Page
    {
        private readonly DisplayMessage _displayMessage;
        private PassengersDetails _selectedPassenger;
        private Window _parentWindow;

        public ContactsPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("Contact");

            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
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
            var passengerUpdatedDetailsList = new List<PassengersDetails>();

            foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
            {
                if (rowItem is PassengersDetails row)
                {
                    passengerUpdatedDetailsList.Add(
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

            FillContactDetailsGrid(passengerUpdatedDetailsList);
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
            PopulateContactDetailsInFields();
        }

        private void PopulateContactDetailsInFields()
        {
            LastNameTextBox.Text = _selectedPassenger.LastName;
            FirstNameTextBox.Text = _selectedPassenger.FirstName;
            MiddleNameTextBox.Text = _selectedPassenger.MiddleName;
            TitleComboBox.Text = _selectedPassenger.Title;

            Line1TextBox.Text = _selectedPassenger.AddressLine1;
            Line2TextBox.Text = _selectedPassenger.AddressLine2;
            PostalTextBox.Text = _selectedPassenger.AddressPostal;
            TownTextBox.Text = _selectedPassenger.AddressTown;
            AddressStateTextBox.Text = _selectedPassenger.AddressState;
            CountryComboBox.Text = _selectedPassenger.AddressCountry;
            MobileTextBox.Text = _selectedPassenger.Mobile;
            EmailTextBox.Text = _selectedPassenger.Email;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields(Line1TextBox.Text, Line2TextBox.Text, PostalTextBox.Text, TownTextBox.Text, AddressStateTextBox.Text, CountryComboBox.Text,
            MobileTextBox.Text, EmailTextBox.Text))
                return;

            SaveOrUpdate(EmailTextBox.Text, MobileTextBox.Text, Line1TextBox.Text, Line2TextBox.Text, PostalTextBox.Text, TownTextBox.Text, AddressStateTextBox.Text, CountryComboBox.Text);

            ClearAllFields(LastNameTextBox, FirstNameTextBox, MiddleNameTextBox, TitleComboBox,
                Line1TextBox, Line2TextBox, PostalTextBox, TownTextBox, AddressStateTextBox, CountryComboBox, MobileTextBox, EmailTextBox);

            ContactDetailsGroupBox.IsEnabled = false;
            SaveButton.IsEnabled = false;
            EditContactButton.IsEnabled = false;

            SharedDataPage.PassengersDetailsGrid = ContactDetailsGrid;
            UpdateMainParentWindow();
        }

        private bool ValidateFields(string addressLine1, string addressLine2, string addressPostal, string addressTown, string addressState, string addressCountry,
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

        private void SaveOrUpdate(string email, string mobile, string addressLine1, string addressLine2, string addressPostal, string addressTown, string addressState, string addressCountry)
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
            }
        }

        private void UpdateMainParentWindow()
        {
            if (SharedDataPage.PassengersDetailsGrid != null)
            {
                _parentWindow = Window.GetWindow(this);
                TextBlock textBlock = _parentWindow.FindName("ContactExpanderTextBlock") as TextBlock;

                // Clear the existing text
                textBlock.Text = string.Empty;

                StringBuilder textBuilder = new StringBuilder();
                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
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