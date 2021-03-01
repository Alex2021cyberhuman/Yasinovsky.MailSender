using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using Yasinovsky.MailSender.Core.Contracts.Services;

namespace Yasinovsky.MailSender.Services.Test
{
    public class SymmetricEncryptServiceTests
    {
        private readonly IEncryptService _service;

        public SymmetricEncryptServiceTests()
        {
            var alg = Aes.Create();
            _service = new SymmetricEncryptService(alg, alg.Key, alg.IV);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task SymmetricEncryptService_EncryptDecryptString_Work(string data)
        {
            var encryptedSting = await _service.EncryptStringAsync(data);

            var decryptedString = await _service.DecryptStringAsync(encryptedSting);

            Assert.Equal(data, decryptedString);
        }

        public static IEnumerable<object[]> Data => Enumerable.Range('a', 'z' - 'a')
            .Select(x => new object[]{new string((char)x, 20)});
    }
}
