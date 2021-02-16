using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Yasinovsky.MailSender.Core.Extensions;
using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Services;

namespace Yasinovsky.MailSender.Tests.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private INetUserService _netUserService;
        private ISmtpClientAccessor _smtpClientAccessor;
        private IEmailSendService _emailSendService;

        public MainWindow()
        {
            InitializeComponent();
            _netUserService = new NetUserService();
            _smtpClientAccessor = new SmtpClientAccessor(_netUserService);
            _emailSendService = new SmtpEmailSendService(_smtpClientAccessor);
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            if ((bool)IsAuthenticateCheck.IsChecked)
            {
                _netUserService.SetCredentials(EmailEdit.Text, PasswordEdit.SecurePassword);
            }
            else
            {
                _netUserService.RemoveCurrentCredentials();
            }
            if (!int.TryParse(PortEdit.Text, out int port))
                return;
            _smtpClientAccessor.SetClient(
                HostEdit.Text,
                port,
                (bool)IsSslCheck.IsChecked);
            try
            {
                var result = await _emailSendService.SendAsync(
                    SubjectEdit.Text,
                    BodyEdit.Text,
                    EmailEdit.Text,
                    EmailToEdit.Text);
                MessageBox.Show(result.ToString());
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, exp.GetType().FullName);
            }
            IsEnabled = true;
        }
    }
}
