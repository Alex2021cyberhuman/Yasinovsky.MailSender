using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;

using MimeKit;
using MimeKit.Text;
using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Core.Enums;
using Yasinovsky.MailSender.Core.Models;
using static MimeKit.Text.TextFormat;

namespace Yasinovsky.MailSender.Services
{

    public class MailKitSmtpEmailSendService : IEmailSendService
    {
        private readonly IEncryptService _encrypt;
        private readonly ILogger<IEmailSendService> _logger;

        private bool IsSecure => _encrypt != null;
        //private readonly ISmtpClientAccessor smtpClientAccessor;

        //public MailKitSmtpEmailSendService(ISmtpClientAccessor smtpClientAccessor)
        //{
        //    this.smtpClientAccessor = smtpClientAccessor;
        //}

        public MailKitSmtpEmailSendService()
        {
        }

        public MailKitSmtpEmailSendService(ILogger<IEmailSendService> logger = null, IEncryptService encrypt = null)
        {
            _encrypt = encrypt;
            _logger = logger;
        }

        //public async Task<EmailSendResult> SendAsync(MailMessage message)
        //{
        //    if (message is null)
        //        throw new ArgumentNullException(nameof(message));
        //    if (!message.To.Any())
        //        throw new ArgumentNullException(nameof(message), nameof(message.To));
        //    if (message.From is null)
        //        throw new ArgumentNullException(nameof(message), nameof(message.From));
        //    if (message.Body is null)
        //        throw new ArgumentNullException(nameof(message), nameof(message.Body));
        //    if (message.Subject is null)
        //        throw new ArgumentNullException(nameof(message), nameof(message.Subject));

        //    var client = smtpClientAccessor.Client;
        //    try
        //    {
        //        await client.SendMailAsync(message);

        //        return EmailSendResult.Success;
        //    }
        //    catch (SmtpException)
        //    {
        //        return EmailSendResult.Unauthorized;
        //    }
        //    catch (TimeoutException)
        //    {
        //        return EmailSendResult.Timeout;
        //    }
        //}

        public async Task<EmailSendResult> SendAsync(Server server, Sender sender, IEnumerable<Recipient> recipients, Message message)
        {
            string password = server.Password;
            if (IsSecure)
            {
               password = await _encrypt.DecryptStringAsync(server.Password);
            }

            var mimeMessage = new MimeMessage
            {
                Subject = message.Title,
                Body = new TextPart(TextFormat.Text)
                {
                    Text = message.Body
                }
            };

            mimeMessage.From.Add(new MailboxAddress(sender.Name, sender.Address));
            IEnumerable<Recipient> enumerable = recipients as List<Recipient> ?? recipients.ToList();
            mimeMessage.To.AddRange(
                enumerable.Select(x =>
                    new MailboxAddress(x.Name, x.Address)));
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(server.Address, server.Port, server.EnableSsl);
                await client.AuthenticateAsync(server.Login, password);
                await client.SendAsync(mimeMessage);

                await client.DisconnectAsync(true);

                _logger.LogInformation($"Success send mail message from {sender} to [{string.Join(", ", enumerable)}] through server: {server}.");
                return EmailSendResult.Success;
            }
            catch (MailKit.ServiceNotAuthenticatedException e)
            {
                _logger?.LogError(e, "Unauthorized sender");
                return EmailSendResult.Unauthorized;
            }
            catch (MailKit.ProtocolException e)
            {
                _logger?.LogError(e, "ProtocolException");
                return EmailSendResult.ProtocolError;
            }
            catch (MailKit.CommandException e)
            {
                _logger?.LogError(e, "CommandException");
                return EmailSendResult.CommandError;
            }
            catch (TimeoutException e)
            {
                _logger?.LogError(e, "TimeoutException");
                return EmailSendResult.Timeout;
            }
        }
    }
}
