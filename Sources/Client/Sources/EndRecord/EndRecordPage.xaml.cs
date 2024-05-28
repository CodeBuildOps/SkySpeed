using SkySpeed.MessageLog;
using SkySpeedService;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkySpeed.EndRecords
{
    /// <summary>
    /// Interaction logic for EndRecordPage.xaml
    /// </summary>
    public partial class EndRecordPage : Page
    {
        private readonly DisplayMessage _displayMessage;
        private readonly SkySpeedServices _skySpeedServices;
        private readonly string _toMail;

        public EndRecordPage()
        {
            InitializeComponent();
            _displayMessage = new DisplayMessage("End record");
            _skySpeedServices = new SkySpeedServices();

            _toMail = GetEmailId();
        }

        private string GetEmailId()
        {
            // Todo: Get email id from datagrid.
            return "abhiksingh1999@gmail.com";
        }

        private void SendItineraryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SendItineraryComboBox.SelectedItem is ContentControl selectedItemControl)
            {
                string selectedItem = selectedItemControl.Content.ToString().ToUpper();

                switch (selectedItem)
                {
                    case "EMAIL":
                        HandleEndRecordSelection("Would you like to send an email?", () => _skySpeedServices.SendEmail(_toMail));
                        break;
                    case "PRINT":
                        //HandleEndRecordSelection("Would you like to print?", () => _skySpeedServices.Print());
                        break;
                    default:
                        SendItineraryComboBox.SelectedItem = null;
                        break;
                }
            }
        }

        private void HandleEndRecordSelection(string message, Action action)
        {
            MessageBoxResult result = _displayMessage.ShowQuestionMessageBox(message);

            if (result == MessageBoxResult.Yes)
            {
                Cursor = Cursors.Wait;
                action();
                Cursor = Cursors.Arrow;
            }
            SendItineraryComboBox.SelectedItem = null;
        }

    }
}
