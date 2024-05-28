using SkySpeed.MessageLog;
using SkySpeedService;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySpeed.Handler;
using System.IO;

namespace SkySpeed.Registration
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {
        private DisplayMessage _displayMessage;
        private SkySpeedServices _skySpeedServices;

        public Signup()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("SkySpeed");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            if (string.IsNullOrEmpty(UserIdTextBox.Text))
            {
                _displayMessage.ShowWarningMessageBox("User Id must not remain empty.");
            }
            else if (string.IsNullOrEmpty(EmployeeNameTextBox.Text))
            {
                _displayMessage.ShowWarningMessageBox("Employee name must not remain empty.");
            }
            else if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                _displayMessage.ShowWarningMessageBox("Password must not remain empty.");
            }
            else if (File.Exists(INIHandler.INIFilePath))
            {
                _skySpeedServices = new SkySpeedServices();
                bool isDBExists = _skySpeedServices.DatabaseExists(INIHandler.INIFilePath);
                if (isDBExists)
                {
                    if (_skySpeedServices.CreateRegistration(UserIdTextBox.Text, PasswordBox.Password, EmployeeNameTextBox.Text))
                    {
                        _displayMessage.ShowSuccessMessageBox("Registered successfully.");
                        Close();
                    }
                    else
                    {
                        _displayMessage.ShowErrorMessageBox("Error occurred in registration. Contact support.");
                    }
                }
                else
                {
                    _displayMessage.ShowWarningMessageBox("Create DBSetup first.");
                    Close();
                }
            }
            else
            {
                _displayMessage.ShowErrorMessageBox("SkySpeed.ini not found. Run DBSetup.");
            }
            Cursor = Cursors.Arrow;
        }
    }
}