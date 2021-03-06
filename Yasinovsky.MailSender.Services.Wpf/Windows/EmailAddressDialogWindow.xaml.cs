using System.Windows;
using System.Windows.Controls;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Services.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for EmailAddressDialogWindow.xaml
    /// </summary>
    public partial class EmailAddressDialogWindow
    {
        public EmailAddressDialogWindow(EmailAddressInfo emailAddress)
        {
            InitializeComponent();
            DataContext = emailAddress;
        }

        private void DialogSaveClose(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void DialogCancelClose(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Validation_OnError(object sender, ValidationErrorEventArgs e)
        {
            var control = (Control) sender;
            if (e.Action == ValidationErrorEventAction.Added)
            {
                control.ToolTip = e.Error.ErrorContent;
            }

            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                control.ClearValue(ToolTipProperty);
            }
            
        }
    }
}
