using SkySpeed.Handler;
using SkySpeed.MessageLog;
using SkySpeedService;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SkySpeedSetup
{
    /// <summary>
    /// Interaction logic for DBSetup.xaml
    /// </summary>
    public partial class DBSetup : Window
    {
        private readonly DisplayMessage _displayMessage;
        private readonly INIHandler _iniHandler;
        private readonly SkySpeedServices _skySpeedServices;

        public DBSetup()
        {
            InitializeComponent();

            Loaded += Page_Loaded;
            _displayMessage = new DisplayMessage("SkySpeed");
            _iniHandler = new INIHandler();
            _skySpeedServices = new SkySpeedServices();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(INIHandler.INIFilePath))
            {
                Dictionary<string, string> iniValues = _skySpeedServices.ReadINI(INIHandler.INIFilePath);
                ServerNameTextBox.Text = iniValues["SERVERNAME"];
                ServerPasswordBox.Password = iniValues["SERVERPASSWORD"];
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            if (string.IsNullOrEmpty(ServerNameTextBox.Text))
            {
                _displayMessage.ShowWarningMessageBox("Server name must not remain empty.");
            }
            else if (string.IsNullOrEmpty(ServerPasswordBox.Password))
            {
                _displayMessage.ShowWarningMessageBox("Sever password must not remain empty.");
            }
            else
            {
                if (_skySpeedServices.CreateINI(ServerUserIdTextBox.Text, ServerNameTextBox.Text, ServerPasswordBox.Password, INIHandler.INIFilePath))
                {
                    _displayMessage.ShowSuccessMessageBox("SkySpeed.ini profile created.");
                }
            }
            Cursor = Cursors.Arrow;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(INIHandler.INIFilePath))
            {
                _displayMessage.ShowErrorMessageBox($"{INIHandler.SkySpeedIni} not found. Create a profile first.");
                return;
            }

            if (!File.Exists(INIHandler.FlightDetailsPath))
            {
                _displayMessage.ShowErrorMessageBox($"{INIHandler.FlightDetailsTxt} not found. Contact support.");
                return;
            }

            Cursor = Cursors.Wait;

            bool isDBExists = _skySpeedServices.DatabaseExists(INIHandler.INIFilePath);
            if (isDBExists)
            {
                _displayMessage.ShowWarningMessageBox("Database already exists. Please log in to SkySpeed.");
            }
            else
            {
                bool isDBSetupDone = _skySpeedServices.CreateDBSetup();
                if (isDBSetupDone)
                {
                    if (_skySpeedServices.InsertFlightDetails(INIHandler.FlightDetailsPath))
                    {
                        _displayMessage.ShowSuccessMessageBox("Database setup has been successfully completed. Please log in to SkySpeed.");

                        // Todo: Enable the message box once it's been tested.
                        //_displayMessage.ShowSuccessMessageBox("Manually copy the SkySpeed.INI file from this client machine to all other client machines to distribute the database parameters.");
                    }
                    else
                    {
                        _displayMessage.ShowErrorMessageBox("Error occurred in creating the database.");
                    }
                }
                else
                {
                    _displayMessage.ShowErrorMessageBox("Error occurred in creating the database.");
                }
            }

            Cursor = Cursors.Arrow;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            bool isDBExists = _skySpeedServices.DatabaseExists(INIHandler.INIFilePath);
            Cursor = Cursors.Arrow;
            if (isDBExists)
            {
                _displayMessage.ShowSuccessMessageBox("Connected successfully.");
            }
            else
            {
                _displayMessage.ShowWarningMessageBox("Can't connect with database.");
            }
        }
    }
}