using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface ISenderUserDialogService
    {
        public Task<Sender> OpenCreateDialogAsync();

        public Task<Sender> OpenEditDialogAsync(Sender sender);
    }


    public interface IRecipientUserDialogService
    {
        public Task<Recipient> OpenCreateDialogAsync();

        public Task<Recipient> OpenEditDialogAsync(Recipient sender);
    }
}