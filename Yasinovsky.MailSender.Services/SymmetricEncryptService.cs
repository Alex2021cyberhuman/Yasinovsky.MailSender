using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Services;

namespace Yasinovsky.MailSender.Services
{
    public class SymmetricEncryptService : IEncryptService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private readonly SymmetricAlgorithm _symmetricAlgorithm;

        public SymmetricEncryptService(SymmetricAlgorithm symmetricAlgorithm, byte[] key, byte[] iv)
        {
            if (iv.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(iv));
            if (key.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(key));
            _key = key;
            _symmetricAlgorithm = symmetricAlgorithm ?? throw new ArgumentNullException(nameof(symmetricAlgorithm));
            _iv = iv;
        }
        public async Task<string> DecryptStringAsync(string value)
        {
            var plainBytes = Convert.FromBase64String(value);
            var encrypt = _symmetricAlgorithm.CreateDecryptor(_key, _iv);
            var encryptBytes = encrypt.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            await Task.CompletedTask;
            return Encoding.UTF8.GetString(encryptBytes);
        }
        public async Task<string> EncryptStringAsync(string value)
        {
            var plainBytes = Encoding.UTF8.GetBytes(value);
            var encrypt = _symmetricAlgorithm.CreateEncryptor(_key, _iv);
            var encryptBytes = encrypt.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            await Task.CompletedTask;
            return Convert.ToBase64String(encryptBytes);
        }
    }
}