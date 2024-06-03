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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SkySpeed.SkySpeedMainWindow
{
    /// <summary>
    /// Interaction logic for SkySpeedWindow.xaml
    /// </summary>
    partial class SkySpeedWindow : Window
    {
        private const string README_FILE_NAME = "Readme\\SkySpeed.pptx";
        private readonly string README_FILE_PATH;
        private readonly DisplayMessage _displayMessage;

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
            OpenApplication(@"C:\Windows\System32\calc.exe", "Calculator");
        }

        private void Notepad_Click(object sender, RoutedEventArgs e)
        {
            OpenApplication(@"C:\Windows\System32\notepad.exe", "Notepad");
        }

        private void OpenApplication(string appPath, string appName)
        {
            if (File.Exists(appPath))
            {
                Process.Start(appPath);
            }
            else
            {
                _displayMessage.ShowErrorMessageBox($"Unable to open {appName}. Contact support.");
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteNextButtonAction();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteBackButtonAction();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.N:
                        ExecuteNextButtonAction();
                        break;
                    case Key.B:
                        ExecuteBackButtonAction();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ExecuteNextButtonAction()
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

        private void ExecuteBackButtonAction()
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

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // Cancel the closing event
            if (_displayMessage.ShowQuestionMessageBox("Closing the application may cause data loss.\nDo you want to end your SkySpeed session?") == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ThemeCheckboxButton_Checked(object sender, RoutedEventArgs e)
        {
            // Define colors for Dark mode
            Color darkHeaderBackground = Colors.Black;
            Color darkSectionBackground = Color.FromRgb(32, 32, 32);
            Color darkMainMenu = Color.FromRgb(32, 32, 32);
            Color darkForeground = Colors.White;

            // Define colors for Light mode
            Color lightHeaderBackground = Color.FromRgb(102, 153, 204);
            Color lightSectionBackground = Color.FromRgb(124, 185, 232);
            Color lightMainMenu = Color.FromRgb(240, 248, 255);
            Color lightForeground = Colors.Black;

            // Determine the selected mode and apply the corresponding theme
            if (ThemeCheckboxButton.IsChecked == true)
            {
                ApplyTheme(darkHeaderBackground, darkSectionBackground, darkMainMenu, darkForeground);
            }
            else
            {
                ApplyTheme(lightHeaderBackground, lightSectionBackground, lightMainMenu, lightForeground);
            }
        }

        private void ApplyTheme(Color headerBackground, Color sectionBackground, Color mainMenu, Color foreground)
        {
            // Update the resource dictionary with the provided colors
            Application.Current.Resources["HeaderBackgroundBrush"] = new SolidColorBrush(headerBackground);
            Application.Current.Resources["SectionBackgroundBrush"] = new SolidColorBrush(sectionBackground);
            Application.Current.Resources["MainMenuBrush"] = new SolidColorBrush(mainMenu);
            Application.Current.Resources["ForegroundBlackBrush"] = new SolidColorBrush(foreground);
        }
    }
}