using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Enums;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface IEmailSendService
    {
        Task<EmailSendResult> SendAsync(Server server, Sender sender, IEnumerable<Recipient> recipients, Message message);
    }
}
