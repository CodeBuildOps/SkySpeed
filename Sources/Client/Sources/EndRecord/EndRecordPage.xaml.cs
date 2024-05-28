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
        private SkySpeedServices _skySpeedServices;
        private readonly string _toMail;

        public EndRecordPage()
        {
            InitializeComponent();

            _displayMessage = new DisplayMessage("End record");
            _toMail = GetEmailId();
        }

        private string GetEmailId()
        {
            // Todo: Get email id from datagrid.
            return "abhiksingh1999@gmail.com";
        }

        private void SendItineraryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(SendItineraryComboBox.SelectedItem is ContentControl selectedItemControl))
                return;

            _skySpeedServices = new SkySpeedServices();
            switch (selectedItemControl.Content.ToString().ToUpper())
            {
                case "EMAIL":
                    HandleEndRecordSelection("Would you like to send an email?", () => _skySpeedServices.SendEmail(_toMail));
                    break;
                case "PRINT":
                    HandleEndRecordSelection("Would you like to print?", () => _skySpeedServices.PrintDocument());
                    break;
            }
        }

        private void HandleEndRecordSelection(string message, Action action)
        {
            if (_displayMessage.ShowQuestionMessageBox(message) == MessageBoxResult.Yes)
            {
                Cursor = Cursors.Wait;
                action();
                Cursor = Cursors.Arrow;
            }
            SendItineraryComboBox.SelectedItem = null;
        }
    }
}