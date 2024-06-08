using Constant;
using SharedData;
using SkySpeed.MessageLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SkySpeed.Passengers
{
    /// <summary>
    /// Interaction logic for PassengersPage.xaml
    /// </summary>
    partial class PassengersPage : Page
    {
        private readonly DisplayMessage _displayMessage;
        private PassengersDetails _selectedPassenger;
        private Window _parentWindow;
        private int _passengerDetailId;

        public PassengersPage()
        {
            InitializeComponent();

            _passengerDetailId = 0;
            _displayMessage = new DisplayMessage("Passenger");

            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
                SetPassengersDetailsGrid();

            SetComboBoxWithCountries();
            SetDatePickerDateRanges();

            // Disable group boxes and buttons initially
            PassengerDetailsGroupBox.IsEnabled = false;
            InfantDetailsGroupBox.IsEnabled = false;
            EditPassengerButton.IsEnabled = false;
            SaveButton.IsEnabled = false;

            // Display passenger counts
            PassengersLabel.Content += $"\tTOTAL: {SharedDataPage.NumberOfPassengers} \t ADT: {SharedDataPage.NumberOfADT} \t CHD: {SharedDataPage.NumberOfCHD}";
        }

        private void SetPassengersDetailsGrid()
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
                            row.SeatPrice,

                            row.PaymentDetailsObject
                            )
                    );
                }
            }

            FillDetailsGrid(passengerUpdatedDetailsList);
        }

        private void SetComboBoxWithCountries()
        {
            var countries = Enum.GetNames(typeof(ConstantHandler.Countries));
            CountryComboBox.ItemsSource = countries;
            InfantCountryComboBox.ItemsSource = countries;
        }

        private void SetDatePickerDateRanges()
        {
            var oneHundredYearsAgo = DateTime.Today.AddYears(-100);

            DOBDatePicker.DisplayDateStart = oneHundredYearsAgo;
            DOBDatePicker.DisplayDateEnd = DateTime.Today;

            InfantDOBDatePicker.DisplayDateStart = oneHundredYearsAgo;
            InfantDOBDatePicker.DisplayDateEnd = DateTime.Today;
        }

        private void AddPassengerButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if there is a selected passenger and deselect if necessary
            if (_selectedPassenger != null)
            {
                // Deselect the selected passenger
                PassengersDetailsGrid.SelectedItem = null;
            }

            Add_Edit_Passenger.Content = "Add Passenger";

            // Enable Passenger Details Group Box if there are adult passengers
            PassengerDetailsGroupBox.IsEnabled = SharedDataPage.NumberOfADT > 0;

            // Enable Infant Details Group Box if there are child passengers
            InfantDetailsGroupBox.IsEnabled = SharedDataPage.NumberOfCHD > 0;

            // Enable Save Button if either Passenger or Infant Group Box is enabled
            SaveButton.IsEnabled = PassengerDetailsGroupBox.IsEnabled || InfantDetailsGroupBox.IsEnabled;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SharedDataPage.NumberOfADT > 0)
            {
                if (!ValidateFields(LastNameTextBox.Text, FirstNameTextBox.Text, TitleComboBox.Text, GenderComboBox.Text))
                    return;

                SaveOrUpdate("ADT", LastNameTextBox.Text, FirstNameTextBox.Text, MiddleNameTextBox.Text,
                                      TitleComboBox.Text, GenderComboBox.Text, DOBDatePicker.Text, CountryComboBox.SelectedItem?.ToString());
                ClearAllFields("ADT");
            }

            if (SharedDataPage.NumberOfCHD > 0)
            {
                if (!ValidateFields(InfantLastNameTextBox.Text, InfantFirstNameTextBox.Text, InfantTitleComboBox.Text, InfantGenderComboBox.Text))
                    return;

                SaveOrUpdate("CHD", InfantLastNameTextBox.Text, InfantFirstNameTextBox.Text, InfantMiddleNameTextBox.Text,
                                      InfantTitleComboBox.Text, InfantGenderComboBox.Text, InfantDOBDatePicker.Text, InfantCountryComboBox.SelectedItem?.ToString());
                ClearAllFields();
            }

            SharedDataPage.PassengersDetailsGrid = PassengersDetailsGrid;
        }

        private bool ValidateFields(string lastName, string firstName, string title, string gender)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                _displayMessage.ShowWarningMessageBox("Last name must not remain empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(firstName))
            {
                _displayMessage.ShowWarningMessageBox("First name must not remain empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(title))
            {
                _displayMessage.ShowWarningMessageBox("Title must not remain empty.");
                return false;
            }
            else if (string.IsNullOrEmpty(gender))
            {
                _displayMessage.ShowWarningMessageBox("Gender must not remain empty.");
                return false;
            }
            return true;
        }

        private void SaveOrUpdate(string passengerType, string lastName, string firstName, string middleName, string title, string gender, string dob, string country)
        {
            // Update the selected passenger's data
            if (_selectedPassenger != null)
            {
                _selectedPassenger.Type = passengerType;
                _selectedPassenger.LastName = lastName;
                _selectedPassenger.FirstName = firstName;
                _selectedPassenger.MiddleName = middleName;
                _selectedPassenger.Title = title;
                _selectedPassenger.Gender = gender;
                _selectedPassenger.FullName = $"{title} {firstName} {middleName} {lastName}";
                _selectedPassenger.DOB = dob;
                _selectedPassenger.Country = country;

                // Refresh the DataGrid to reflect the changes
                PassengersDetailsGrid.Items.Refresh();

                // Deselect the selected passenger
                PassengersDetailsGrid.SelectedItem = null;
            }
            // Add a new passenger
            else
            {
                _passengerDetailId = PassengersDetailsGrid.Items.Count + 1;
                FillDetailsGrid(new List<PassengersDetails>() {
                    new PassengersDetails (
                        _passengerDetailId,
                        passengerType,
                        gender,
                        title,
                        firstName,
                        middleName,
                        lastName,
                        dob,
                        country
                     )
                });
            }

            SharedDataPage.PassengersDetailsGrid = PassengersDetailsGrid;
            UpdateMainParentWindow();
        }

        private void FillDetailsGrid(List<PassengersDetails> passengerData)
        {
            foreach (var item in passengerData)
            {
                PassengersDetailsGrid.Items.Add(item);
            }
        }

        private void PassengerDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedPassenger = PassengersDetailsGrid.SelectedItem as PassengersDetails;
            EditPassengerButton.IsEnabled = _selectedPassenger != null;
        }

        private void EditPassengerButton_Click(object sender, RoutedEventArgs e)
        {
            Add_Edit_Passenger.Content = "Edit Passenger";

            SaveButton.IsEnabled = true;

            if (_selectedPassenger.Type == "ADT")
            {
                PopulateDetails(_selectedPassenger, LastNameTextBox, FirstNameTextBox, MiddleNameTextBox, TitleComboBox, GenderComboBox, DOBDatePicker, CountryComboBox);
                PassengerDetailsGroupBox.IsEnabled = true;
                InfantDetailsGroupBox.IsEnabled = false;
            }
            else
            {
                PopulateDetails(_selectedPassenger, InfantLastNameTextBox, InfantFirstNameTextBox, InfantMiddleNameTextBox, InfantTitleComboBox, InfantGenderComboBox, InfantDOBDatePicker, InfantCountryComboBox);
                PassengerDetailsGroupBox.IsEnabled = false;
                InfantDetailsGroupBox.IsEnabled = true;
            }
        }

        private void PopulateDetails(PassengersDetails selectedPassenger, TextBox lastNameTextBox, TextBox firstNameTextBox, TextBox middleNameTextBox, ComboBox titleComboBox, ComboBox genderComboBox, DatePicker dobDatePicker, ComboBox countryComboBox)
        {
            lastNameTextBox.Text = selectedPassenger.LastName;
            firstNameTextBox.Text = selectedPassenger.FirstName;
            middleNameTextBox.Text = selectedPassenger.MiddleName;
            titleComboBox.Text = selectedPassenger.Title;
            genderComboBox.Text = selectedPassenger.Gender;
            dobDatePicker.Text = selectedPassenger.DOB;
            countryComboBox.Text = selectedPassenger.Country;
        }

        private void ClearAllFields(string type = "CHD")
        {
            if (type == "ADT")
            {
                ClearPassengerFields(LastNameTextBox, FirstNameTextBox, MiddleNameTextBox, TitleComboBox, GenderComboBox, DOBDatePicker, CountryComboBox);
                PassengerDetailsGroupBox.IsEnabled = false;
            }
            else
            {
                ClearPassengerFields(InfantLastNameTextBox, InfantFirstNameTextBox, InfantMiddleNameTextBox, InfantTitleComboBox, InfantGenderComboBox, InfantDOBDatePicker, InfantCountryComboBox);
                InfantDetailsGroupBox.IsEnabled = false;
            }

            SaveButton.IsEnabled = false;
            EditPassengerButton.IsEnabled = false;
        }

        private void ClearPassengerFields(params Control[] controls)
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

        private void UpdateMainParentWindow()
        {
            if (SharedDataPage.PassengersDetailsGrid?.Items != null)
            {
                _parentWindow = Window.GetWindow(this);
                TextBlock textBlock = _parentWindow.FindName("PassengersExpanderTextBlock") as TextBlock;

                // Clear the existing text
                textBlock.Text = string.Empty;

                StringBuilder textBuilder = new StringBuilder();
                foreach (var rowItem in SharedDataPage.PassengersDetailsGrid.Items)
                {
                    if (rowItem is PassengersDetails row)
                    {
                        textBuilder.AppendLine($"Passenger - {row.PassengerId}");
                        textBuilder.AppendLine($"{row.Type}\t{row.Gender}");
                        textBuilder.AppendLine($"{row.FullName}");
                        textBuilder.AppendLine($"DOB: {row.DOB}  {row.Country}");
                        textBuilder.AppendLine();
                    }
                }

                // Update the TextBlock with the new text
                textBlock.Text = textBuilder.ToString();
            }
        }
    }
}