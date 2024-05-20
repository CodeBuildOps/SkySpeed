using Constant;
using SharedData;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SkySpeed.Contacts
{
    /// <summary>
    /// Interaction logic for ContactsPage.xaml
    /// </summary>
    public partial class ContactsPage : Page
    {
        private DisplayMessage _displayMessage;
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
            // ContactGrid contains more information than PassengerGrid
            var detailsList = new List<PassengersDetails>();

            var gridItems = SharedDataPage.ContactDetailsGrid?.Items ?? SharedDataPage.PassengersDetailsGrid.Items;

            if (SharedDataPage.ContactDetailsGrid != null)
            {
                foreach (var rowItem in SharedDataPage.ContactDetailsGrid.Items)
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
                                row.Email
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
                                row.Email
                                )
                        );
                    }
                }
            }

            FillContactDetailsGrid(detailsList);
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
            else if (string.IsNullOrEmpty(addressCountry))
            {
                _displayMessage.ShowWarningMessageBox("Address country must not remain empty.");
                return false;
            }

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
            string fullAddress = $"{Line1TextBox.Text}, {Line2TextBox.Text}, {PostalTextBox.Text}, {TownTextBox.Text}, {AddressStateTextBox.Text}, {CountryComboBox.Text}";

            // Update the selected passenger's contact data
            _selectedPassenger.Email = email;
            _selectedPassenger.Mobile = mobile;
            _selectedPassenger.FullAddress = fullAddress;

            // Hidden columns
            _selectedPassenger.AddressLine1 = addressLine1;
            _selectedPassenger.AddressLine2 = addressLine2;
            _selectedPassenger.AddressPostal = addressPostal;
            _selectedPassenger.AddressTown = addressTown;
            _selectedPassenger.AddressState = addressState;
            _selectedPassenger.AddressCountry = addressCountry;

            // Refresh the DataGrid to reflect the changes
            ContactDetailsGrid.Items.Refresh();

            // Deselect the selected passenger
            ContactDetailsGrid.SelectedItem = null;

            // Update the Main Parent window
            _parentWindow = Window.GetWindow(this);
            TextBlock textBlock = (TextBlock)_parentWindow.FindName("ContactExpanderTextBlock");
            textBlock.Text += $"{mobile}\n{email}\n{fullAddress}\n\n";

            SharedDataPage.ContactDetailsGrid = ContactDetailsGrid;
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
    }
}