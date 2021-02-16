using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Core.Enums;

namespace Yasinovsky.MailSender.Core.Extensions
{
    public static class EmailSendServiceExtensions
    {
        public static async Task<EmailSendResult> SendAsync(this IEmailSendService service, string title, string body, string from, IEnumerable<string> to)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = title,
                Body = body,
                IsBodyHtml = Regex.IsMatch(body, @"<.+>.*</.+>")
            };
            foreach (var item in to)
                mailMessage.To.Add(item);
            return await service.SendAsync(mailMessage);
        }

        public static async Task<EmailSendResult> SendAsync(this IEmailSendService service, string title, string body, string from, string to)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = title,
                Body = body,
                IsBodyHtml = Regex.IsMatch(body, @"<.+>.*</.+>")
            };
            mailMessage.To.Add(to);
            return await service.SendAsync(mailMessage);
        }
    }

    public static class SmtpClientAccessorExtensions
    {
        public static void SetClient(this ISmtpClientAccessor service, string host, int port, bool useSsl = true)
        {
            service.SetClient(new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = useSsl
            });
        }
    }
}
