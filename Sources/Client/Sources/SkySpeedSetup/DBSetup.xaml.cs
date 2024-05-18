using SkySpeed.Handler;
using SkySpeed.MessageLog;
using SkySpeedService;
using System.Windows;
using System.IO;
using System.Windows.Input;

namespace SkySpeedSetup
{
    /// <summary>
    /// Interaction logic for DBSetup.xaml
    /// </summary>
    public partial class DBSetup : Window
    {
        private DisplayMessage _displayMessage;
        private INIHandler _iniHandler;
        private SkySpeedServices _skySpeedServices;

        public DBSetup()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("SkySpeed");
            _iniHandler = new INIHandler();
            _skySpeedServices = new SkySpeedServices();
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
            Cursor = Cursors.Wait;
            if (File.Exists(INIHandler.INIFilePath))
            {
                bool isDBExists = _skySpeedServices.DatabaseExists(INIHandler.INIFilePath);
                if (!isDBExists)
                {
                    bool isDBSetupDone = _skySpeedServices.CreateDBSetup();
                    if (isDBSetupDone)
                    {
                        _displayMessage.ShowSuccessMessageBox("Database created successfully.");
                    }
                    else
                    {
                        _displayMessage.ShowErrorMessageBox("Error occurred in creating the database.");
                    }
                }
                else
                {
                    _displayMessage.ShowWarningMessageBox("Database exists.");
                }
            }
            else
            {
                _displayMessage.ShowErrorMessageBox("Create a profile first.");
            }
            Cursor = Cursors.Arrow;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            bool isDBExists = _skySpeedServices.DatabaseExists(INIHandler.INIFilePath);
            Cursor = Cursors.Arrow;
            if (isDBExists)
                _displayMessage.ShowSuccessMessageBox("Connected successfully.");
            else
                _displayMessage.ShowWarningMessageBox("Can't connect with database.");
        }
    }
}