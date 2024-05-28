using Constant;
using SharedData;
using SkySpeed.MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SkySpeed.AvailabilityRequest
{
    /// <summary>
    /// Interaction logic for AvailabilityRequestPage.xaml
    /// </summary>
    public partial class AvailabilityRequestPage : Page
    {
        private readonly DisplayMessage _displayMessage;

        public AvailabilityRequestPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("SkySpeed");

            SetDatePickerDateRanges();
            SetComboBoxWithCaptials();
            SetComboBoxWithTime();
            SetComboBoxWithNumberOfPassengers();

            NumberOfPassengersLabel.Content = SharedDataPage.NumberOfPassengers;
            ADTComboBox.SelectedItem = SharedDataPage.NumberOfADT;
            CHDComboBox.SelectedItem = SharedDataPage.NumberOfCHD;
        }

        private void SetDatePickerDateRanges()
        {
            var twoMonthsFromToday = DateTime.Today.AddMonths(2);

            DatePicker.DisplayDateStart = DateTime.Today;
            DatePicker.DisplayDateEnd = twoMonthsFromToday;

            ReturnDatePicker.DisplayDateStart = DateTime.Today;
            ReturnDatePicker.DisplayDateEnd = twoMonthsFromToday;

            OneWayRoundTripCalendar.DisplayDateStart = DateTime.Today;
            OneWayRoundTripCalendar.DisplayDateEnd = twoMonthsFromToday;
        }

        private void SetComboBoxWithCaptials()
        {
            var capitals = Enum.GetNames(typeof(ConstantHandler.Capital));
            FromComboBox.ItemsSource = capitals;
            ToComboBox.ItemsSource = capitals;
        }

        private void SetComboBoxWithTime()
        {
            var times = GetTimes();
            OutboundFlightTime.ItemsSource = times;
            ReturnFlightTime.ItemsSource = times;
        }

        private List<string> GetTimes()
        {
            List<string> times = new List<string>();
            for (int i = 0; i < 48; i++)
            {
                int hour = i / 2;
                int minute = i % 2 == 0 ? 0 : 30;
                string amPm = hour < 12 ? "AM" : (hour == 12 ? "PM" : "PM");
                times.Add($"{hour:D2}:{minute:D2} {amPm}");
            }
            return times;
        }

        private void SetComboBoxWithNumberOfPassengers()
        {
            ADTComboBox.ItemsSource = CHDComboBox.ItemsSource = Enumerable.Range(1, 9);
        }

        private void ToComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ToComboBox.SelectedItem != null && FromComboBox.SelectedItem != null)
            {
                if (ToComboBox.SelectedItem.ToString() == FromComboBox.SelectedItem.ToString())
                {
                    _displayMessage.ShowWarningMessageBox("Destination should be distinct.");
                    ToComboBox.SelectedItem = null;
                }
            }
        }

        private void ADTComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePassengerCount();
        }

        private void CHDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePassengerCount();
        }

        private void UpdatePassengerCount()
        {
            int adtCount = ADTComboBox.SelectedItem != null ? Convert.ToInt32(ADTComboBox.SelectedItem.ToString()) : 0;
            int chdCount = CHDComboBox.SelectedItem != null ? Convert.ToInt32(CHDComboBox.SelectedItem.ToString()) : 0;
            NumberOfPassengersLabel.Content = adtCount + chdCount;

            SharedDataPage.NumberOfPassengers = adtCount + chdCount;
            SharedDataPage.NumberOfADT = adtCount;
            SharedDataPage.NumberOfCHD = chdCount;
        }
    }
}