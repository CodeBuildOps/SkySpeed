using System.Windows;

namespace SkySpeed.MessageLog
{
    public class DisplayMessage
    {
        private readonly string MSGHEADER;
        public DisplayMessage(string msgHeader)
        {
            MSGHEADER = msgHeader;
        }
        public void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, MSGHEADER, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowWarningMessageBox(string message)
        {
            MessageBox.Show(message, MSGHEADER, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        public void ShowSuccessMessageBox(string message)
        {
            MessageBox.Show(message, MSGHEADER, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}