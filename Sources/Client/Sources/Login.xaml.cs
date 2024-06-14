using SkySpeed.Handler;
using SkySpeed.MessageLog;
using SkySpeed.Registration;
using SkySpeed.SkySpeedMainWindow;
using SkySpeedService;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkySpeed
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private Signup _signup;
        private readonly DisplayMessage _displayMessage;
        private SkySpeedServices _skySpeedServices;
        private SkySpeedWindow _mainSkySpeed;

        public Login()
        {
            InitializeComponent();

            // Simply to initialize
            _ = new INIHandler();
            _displayMessage = new DisplayMessage("SkySpeed");
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            _signup = new Signup();
            _signup.ShowDialog();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            if (string.IsNullOrEmpty(UserIdTextBox.Text))
            {
                _displayMessage.ShowWarningMessageBox("User Id must not remain empty.");
            }
            else if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                _displayMessage.ShowWarningMessageBox("Password must not remain empty.");
            }
            else
            {
                if (File.Exists(INIHandler.INIFilePath))
                {
                    _skySpeedServices = new SkySpeedServices();
                    bool isDBExists = _skySpeedServices.DatabaseExists(INIHandler.INIFilePath);
                    if (isDBExists)
                    {
                        bool loginSuccess = _skySpeedServices.DoLogin(UserIdTextBox.Text, PasswordBox.Password);
                        if (loginSuccess)
                        {
                            _mainSkySpeed = new SkySpeedWindow();
                            _mainSkySpeed.Show();
                            Close();
                        }
                        else
                        {
                            _displayMessage.ShowErrorMessageBox("Login failed.");
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
            }
            Cursor = Cursors.Arrow;
        }
    }
}