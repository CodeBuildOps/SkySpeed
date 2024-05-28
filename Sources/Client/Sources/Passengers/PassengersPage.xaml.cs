using Constant;
using SharedData;
using SkySpeed.MessageLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SkySpeed.Passengers
{
    /// <summary>
    /// Interaction logic for PassengersPage.xaml
    /// </summary>
    public partial class PassengersPage : Page
    {
        private DisplayMessage _displayMessage;
        private PassengersDetails _selectedPassenger;
        private Window _parentWindow;

        public PassengersPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("Passenger");

            if (SharedDataPage.PassengersDetailsGrid != null)
                SetPassengersDetailsGrid();

            SetComboBoxWithCountries();
            SetComboBoxWithNationalities();
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
                            row.Country
                            )
                    );
                }
            }

            FillPassengersDetailsGrid(passengersDetailsList);
        }

        private void SetComboBoxWithCountries()
        {
            var countries = Enum.GetNames(typeof(ConstantHandler.Countries));
            CountryComboBox.ItemsSource = countries;
            InfantCountryComboBox.ItemsSource = countries;
        }

        private void SetComboBoxWithNationalities()
        {
            var nationalities = Enum.GetNames(typeof(ConstantHandler.Nationality));
            NatonalityComboBox.ItemsSource = nationalities;
            InfantNatonalityComboBox.ItemsSource = nationalities;
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
                if (!ValidatePassengerFields(LastNameTextBox.Text, FirstNameTextBox.Text, TitleComboBox.Text, GenderComboBox.Text))
                    return;

                SaveOrUpdatePassenger("ADT", _selectedPassenger, LastNameTextBox.Text, FirstNameTextBox.Text, MiddleNameTextBox.Text,
                                      TitleComboBox.Text, GenderComboBox.Text, DOBDatePicker.Text, NatonalityComboBox.SelectedItem?.ToString(),
                                      CountryComboBox.SelectedItem?.ToString());
                ClearAllFields("ADT");
            }

            if (SharedDataPage.NumberOfCHD > 0)
            {
                if (!ValidatePassengerFields(InfantLastNameTextBox.Text, InfantFirstNameTextBox.Text, InfantTitleComboBox.Text, InfantGenderComboBox.Text))
                    return;

                SaveOrUpdatePassenger("CHD", _selectedPassenger, InfantLastNameTextBox.Text, InfantFirstNameTextBox.Text, InfantMiddleNameTextBox.Text,
                                      InfantTitleComboBox.Text, InfantGenderComboBox.Text, InfantDOBDatePicker.Text, InfantNatonalityComboBox.SelectedItem?.ToString(),
                                      InfantCountryComboBox.SelectedItem?.ToString());
                ClearAllFields();
            }

            SharedDataPage.PassengersDetailsGrid = PassengersDetailsGrid;
        }

        private bool ValidatePassengerFields(string lastName, string firstName, string title, string gender)
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

        private void SaveOrUpdatePassenger(string passengerType, PassengersDetails selectedPassenger, string lastName, string firstName, string middleName,
                                           string title, string gender, string dob, string nationality, string country)
        {
            // Update the selected passenger's data
            if (selectedPassenger != null)
            {
                selectedPassenger.Type = passengerType;
                selectedPassenger.Gender = gender;
                selectedPassenger.FullName = $"{title} {firstName} {middleName} {lastName}";
                selectedPassenger.DOB = dob;
                selectedPassenger.Nationality = nationality;
                selectedPassenger.Country = country;

                // Refresh the DataGrid to reflect the changes
                PassengersDetailsGrid.Items.Refresh();

                // Deselect the selected passenger
                PassengersDetailsGrid.SelectedItem = null;

            }
            // Add a new passenger
            else
            {
                FillPassengersDetailsGrid(new List<PassengersDetails>() {
                    new PassengersDetails (
                        passengerType,
                        gender,
                        title,
                        firstName,
                        middleName,
                        lastName,
                        dob,
                        nationality,
                        country
                     )
                });
            }

            // Update the Main Parent window
            _parentWindow = Window.GetWindow(this);
            TextBlock textBlock = (TextBlock)_parentWindow.FindName("PassengersExpanderTextBlock");
            textBlock.Text += $"{passengerType}\t{gender}\n{$"{title} {firstName} {middleName} {lastName}"}  {dob}  {nationality}  {country}\n\n";
        }

        private void FillPassengersDetailsGrid(List<PassengersDetails> passengerData)
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
                PopulatePassengerDetails(_selectedPassenger, LastNameTextBox, FirstNameTextBox, MiddleNameTextBox, TitleComboBox, GenderComboBox, DOBDatePicker, NatonalityComboBox, CountryComboBox);
                PassengerDetailsGroupBox.IsEnabled = true;
                InfantDetailsGroupBox.IsEnabled = false;
            }
            else
            {
                PopulatePassengerDetails(_selectedPassenger, InfantLastNameTextBox, InfantFirstNameTextBox, InfantMiddleNameTextBox, InfantTitleComboBox, InfantGenderComboBox, InfantDOBDatePicker, InfantNatonalityComboBox, InfantCountryComboBox);
                PassengerDetailsGroupBox.IsEnabled = false;
                InfantDetailsGroupBox.IsEnabled = true;
            }
        }

        private void PopulatePassengerDetails(PassengersDetails selectedPassenger, TextBox lastNameTextBox, TextBox firstNameTextBox, TextBox middleNameTextBox, ComboBox titleComboBox, ComboBox genderComboBox, DatePicker dobDatePicker, ComboBox nationalityComboBox, ComboBox countryComboBox)
        {
            lastNameTextBox.Text = selectedPassenger.LastName;
            firstNameTextBox.Text = selectedPassenger.FirstName;
            middleNameTextBox.Text = selectedPassenger.MiddleName;
            titleComboBox.Text = selectedPassenger.Title;
            genderComboBox.Text = selectedPassenger.Gender;
            dobDatePicker.Text = selectedPassenger.DOB;
            nationalityComboBox.Text = selectedPassenger.Nationality;
            countryComboBox.Text = selectedPassenger.Country;
        }

        private void ClearAllFields(string type = "CHD")
        {
            if (type == "ADT")
            {
                ClearPassengerFields(LastNameTextBox, FirstNameTextBox, MiddleNameTextBox, TitleComboBox, GenderComboBox, DOBDatePicker, NatonalityComboBox, CountryComboBox);
                PassengerDetailsGroupBox.IsEnabled = false;
            }
            else
            {
                ClearPassengerFields(InfantLastNameTextBox, InfantFirstNameTextBox, InfantMiddleNameTextBox, InfantTitleComboBox, InfantGenderComboBox, InfantDOBDatePicker, InfantNatonalityComboBox, InfantCountryComboBox);
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
    }
}