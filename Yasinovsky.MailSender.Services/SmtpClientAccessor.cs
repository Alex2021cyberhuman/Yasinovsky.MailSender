using System.Net.Mail;

using Yasinovsky.MailSender.Core.Contracts.Services;

namespace Yasinovsky.MailSender.Services
{
    public class SmtpClientAccessor
    {
        //private readonly INetUserService _netUserService;

        //public SmtpClientAccessor(INetUserService netUserService)
        //{
        //    this._netUserService = netUserService;
        //}

        private SmtpClient _smtpClient;

        public SmtpClient Client
        {
            get
            {
                //if (_smtpClient is null)
                //    return null;
                ////var credentials = _netUserService?.GetCredentials();
                //if (credentials != null)
                //    _smtpClient.Credentials = credentials;
                //else
                //    _smtpClient.UseDefaultCredentials = true;
                return _smtpClient;
            }
        }

        public void SetClient(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }
    }
}
