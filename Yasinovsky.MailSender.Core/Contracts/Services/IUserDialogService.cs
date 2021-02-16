using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface IUserDialogService
    {
        public Task ShowInformationAsync(string message, string caption);

        public Task ShowWarningAsync(string message, string caption);

        public Task ShowErrorAsync(string message, string caption);

        public Task<bool> AskBoolAsync(string message, string caption);

        public Task<string > AskStringAsync(string message, string caption, string defaultValue = null);
    }
}
