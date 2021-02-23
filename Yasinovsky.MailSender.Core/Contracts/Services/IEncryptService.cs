using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface IEncryptService
    {
        public Task<string> DecryptStringAsync(string value);
        public Task<string> EncryptStringAsync(string value);
    }
}