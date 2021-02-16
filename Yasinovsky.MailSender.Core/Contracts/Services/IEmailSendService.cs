using System.Net.Mail;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Enums;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface IEmailSendService
    {
        Task<EmailSendResult> SendAsync(MailMessage message);
    }
}
