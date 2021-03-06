using System;
using System.Net;
using System.Security;

namespace Yasinovsky.MailSender.Services
{
    public class NetUserService
    {
        private NetworkCredential _current = null;

        public NetworkCredential GetCredentials() => _current;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
        public void RemoveCurrentCredentials()
        {
            _current?.SecurePassword?.Dispose();
            if (_current != null)
            {
                _current.SecurePassword = null;
                _current.Domain = null;
                _current.UserName = null;
                GC.SuppressFinalize(_current);
            }
            _current = null;
        }

        public void SetCredentials(string username, SecureString password, string domain = null)
        {
            RemoveCurrentCredentials();
            _current = new NetworkCredential()
            {
                UserName = username,
                SecurePassword = password,
                Domain = domain
            };
        }
        public void SetCredentials(string username, string plainPassword, string domain = null)
        {
            RemoveCurrentCredentials();
            var secureString = new SecureString();
            foreach (var item in plainPassword)
            {
                secureString.AppendChar(item);
            }

            _current = new NetworkCredential()
            {
                UserName = username,
                SecurePassword = secureString,
                Domain = domain
            };
        }
    }
}
