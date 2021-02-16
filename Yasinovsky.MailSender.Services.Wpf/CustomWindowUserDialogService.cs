using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Services.Wpf.ViewModels;
using Yasinovsky.MailSender.Services.Wpf.Windows;

namespace Yasinovsky.MailSender.Services.Wpf
{
    public class CustomWindowUserDialogService : IUserDialogService
    {
        public Task ShowInformationAsync(string message, string caption)
        {
            return CreateMessageTask(message, caption, 0);
        }

        public Task ShowWarningAsync(string message, string caption)
        {
            return CreateMessageTask(message, caption, 1);
        }

        public Task ShowErrorAsync(string message, string caption)
        {
            return CreateMessageTask(message, caption, 2);
        }
        private Task CreateMessageTask(string message, string caption, int status) =>
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var window = new MessageWindow(new MessageViewModel(caption, message, status));
                window.ShowDialog();
            }).Task;

        public Task<bool> AskBoolAsync(string message, string caption)
        {
            throw new NotImplementedException();
        }

        public Task<string> AskStringAsync(string message, string caption, string defaultValue = null)
        {
            throw new NotImplementedException();
        }
    }
}
