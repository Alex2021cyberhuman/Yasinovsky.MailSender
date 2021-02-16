using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Core.Enums;

namespace Yasinovsky.MailSender.Services
{

    public class SmtpEmailSendService : IEmailSendService
    {
        private readonly ISmtpClientAccessor smtpClientAccessor;

        public SmtpEmailSendService(ISmtpClientAccessor smtpClientAccessor)
        {
            this.smtpClientAccessor = smtpClientAccessor;
        }

        public async Task<EmailSendResult> SendAsync(MailMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (!message.To.Any())
                throw new ArgumentNullException(nameof(message), nameof(message.To));
            if (message.From is null)
                throw new ArgumentNullException(nameof(message), nameof(message.From));
            if (message.Body is null)
                throw new ArgumentNullException(nameof(message), nameof(message.Body));
            if (message.Subject is null)
                throw new ArgumentNullException(nameof(message), nameof(message.Subject));

            var client = smtpClientAccessor.Client;
            try
            {
                await client.SendMailAsync(message);

                return EmailSendResult.Success;
            }
            catch (SmtpException)
            {
                return EmailSendResult.Unauthorized;
            }
            catch (TimeoutException)
            {
                return EmailSendResult.Timeout;
            }
        }
    }
}
