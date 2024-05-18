using SkySpeed.AvailabilityRequest;
using SkySpeed.Contacts;
using SkySpeed.EndRecords;
using SkySpeed.FlightResults;
using SkySpeed.MessageLog;
using SkySpeed.Passengers;
using SkySpeed.Payments;
using SkySpeed.ReservationSummary;
using SkySpeed.SeatMap;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SkySpeed.SkySpeedMainWindow
{
    /// <summary>
    /// Interaction logic for SkySpeedWindow.xaml
    /// </summary>
    public partial class SkySpeedWindow : Window
    {
        private const string README_FILE_NAME = "Readme\\SkySpeed.pptx";
        private readonly string README_FILE_PATH;
        private DisplayMessage _displayMessage;

        public SkySpeedWindow()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("SkySpeed");
            README_FILE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, README_FILE_NAME);

            // Show the first page
            NavigationFrame.Content = new AvailabilityRequestPage();
            BackButton.IsEnabled = false;
        }

        private void BookingMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new AvailabilityRequestPage());
            NextButton.IsEnabled = true;
            BackButton.IsEnabled = false;
        }

        private void AboutSkySpeed_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(README_FILE_PATH))
                Process.Start(README_FILE_PATH);
            else
                _displayMessage.ShowErrorMessageBox("ReadMe file not found.");
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Windows\System32\calc.exe");
        }

        private void Notepad_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\Windows\System32\notepad.exe");
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            // Navigate next page
            // 1 page
            if (NavigationFrame.Content?.GetType() == typeof(AvailabilityRequestPage))
            {
                NavigationFrame.Navigate(new FlightResultsPage());
                BackButton.IsEnabled = true;
            }
            // 2 page
            else if (NavigationFrame.Content?.GetType() == typeof(FlightResultsPage))
            {
                NavigationFrame.Navigate(new PassengersPage());
            }
            // 3 page
            else if (NavigationFrame.Content?.GetType() == typeof(PassengersPage))
            {
                NavigationFrame.Navigate(new ContactsPage());
            }
            // 4 page
            else if (NavigationFrame.Content?.GetType() == typeof(ContactsPage))
            {
                NavigationFrame.Navigate(new SeatMapPage());
            }
            // 5 page
            else if (NavigationFrame.Content?.GetType() == typeof(SeatMapPage))
            {
                NavigationFrame.Navigate(new PaymentsPage());
            }
            // 6 page
            else if (NavigationFrame.Content?.GetType() == typeof(PaymentsPage))
            {
                NavigationFrame.Navigate(new EndRecordPage());
            }
            // 7 page
            else if (NavigationFrame.Content?.GetType() == typeof(EndRecordPage))
            {
                NavigationFrame.Navigate(new ReservationSummaryPage());
                NextButton.IsEnabled = false;
            }
            Cursor = Cursors.Arrow;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Arrow;
            // Navigate back page
            // 1 page
            if (NavigationFrame.Content?.GetType() == typeof(FlightResultsPage))
            {
                NavigationFrame.Navigate(new AvailabilityRequestPage());
                BackButton.IsEnabled = false;
                NextButton.IsEnabled = true;
            }
            // 2 page
            else if (NavigationFrame.Content?.GetType() == typeof(PassengersPage))
            {
                NavigationFrame.Navigate(new FlightResultsPage());
            }
            // 3 page
            else if (NavigationFrame.Content?.GetType() == typeof(ContactsPage))
            {
                NavigationFrame.Navigate(new PassengersPage());
            }
            // 4 page
            else if (NavigationFrame.Content?.GetType() == typeof(SeatMapPage))
            {
                NavigationFrame.Navigate(new ContactsPage());
            }
            // 5 page
            else if (NavigationFrame.Content?.GetType() == typeof(PaymentsPage))
            {
                NavigationFrame.Navigate(new SeatMapPage());
            }
            // 6 page
            else if (NavigationFrame.Content?.GetType() == typeof(EndRecordPage))
            {
                NavigationFrame.Navigate(new PaymentsPage());
            }
            // 7 page
            else if (NavigationFrame.Content?.GetType() == typeof(ReservationSummaryPage))
            {
                NavigationFrame.Navigate(new EndRecordPage());
                NextButton.IsEnabled = true;
            }
            Cursor = Cursors.Arrow;
        }
    }
}