using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface INetUserService
    {
        NetworkCredential GetCredentials();

        void SetCredentials(string username, string plainPassword, string domain = default);

        void SetCredentials(string username, SecureString password, string domain = default);
        void RemoveCurrentCredentials();
    }
}
