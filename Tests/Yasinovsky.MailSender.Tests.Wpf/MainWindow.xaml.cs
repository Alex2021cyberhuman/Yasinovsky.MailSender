﻿using System.Windows;

namespace Yasinovsky.MailSender.Tests.Wpf
{
    public partial class MainWindow
    {
        //private readonly INetUserService _netUserService;
        //private readonly ISmtpClientAccessor _smtpClientAccessor;
        //private readonly IUserDialogService _userDialogService;
        public MainWindow()
        {
            InitializeComponent();
            //_netUserService = new NetUserService();
            //_smtpClientAccessor = new SmtpClientAccessor(_netUserService);
            //_emailSendService = new MailKitSmtpEmailSendService(_smtpClientAccessor);
            /*userDialogService = new CustomWindowUserDialogService();*/
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    IsEnabled = false;
                //if (IsAuthenticateCheck.IsChecked != null && (bool) IsAuthenticateCheck.IsChecked)
                //    _netUserService.SetCredentials(EmailEdit.Text, PasswordEdit.SecurePassword);
                //else
                //    _netUserService.RemoveCurrentCredentials();
                //if (!int.TryParse(PortEdit.Text, out var port))
                //    return;
                //_smtpClientAccessor.SetClient(
                //    HostEdit.Text,
                //port,
                //IsSslCheck.IsChecked != null && (bool)IsSslCheck.IsChecked);
            
                //var result = await _emailSendService.SendAsync(
                //    SubjectEdit.Text,
                //    BodyEdit.Text,
                //    EmailEdit.Text,
                //    EmailToEdit.Text);
                //if (result == EmailSendResult.Success)
                //{
                //    await _userDialogService.ShowInformationAsync("Email send success", "Info");
                //}
                //else if (result == EmailSendResult.Unauthorized)
                //{
                //    await _userDialogService.ShowWarningAsync("Unauthorized", "Warning");
                //}
                //else
                //{
                //    await _userDialogService.ShowErrorAsync("Timeout", "Error");
                //}
            //}
            //catch (Exception exp)
            //{
            //    await _userDialogService.ShowErrorAsync($"{exp.Message}\n{exp.StackTrace}", exp.GetType().FullName);
            //}
            //IsEnabled = true;
        }
    }
}
