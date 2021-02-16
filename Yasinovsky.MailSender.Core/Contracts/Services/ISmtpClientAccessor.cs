using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface ISmtpClientAccessor
    {
        SmtpClient Client { get; }

        void SetClient(SmtpClient smtpClient);
    }
}
