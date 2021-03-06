using System.Windows;
using System.Windows.Controls;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Services.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for ServerDialogWindow.xaml
    /// </summary>
    public partial class ServerDialogWindow : Window
    {
        public ServerDialogWindow(Server server)
        {
            InitializeComponent();
            DataContext = server;
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
            var control = (Control)sender;
            if (e.Action == ValidationErrorEventAction.Added)
            {
                control.ToolTip = e.Error.ErrorContent;
            }

            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                control.ClearValue(ToolTipProperty);
            }
        }

        private void PasswordEdit_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            //Text="{Binding Path=Password, UpdateSourceTrigger=PropertyChanged, ValidatesOnDataErrors=True, NotifyOnValidationError=True}"
            var viewModel = (Server) DataContext;
            var password = ((PasswordBox) sender).Password;
            viewModel.Password = password;
        }
    }
}
