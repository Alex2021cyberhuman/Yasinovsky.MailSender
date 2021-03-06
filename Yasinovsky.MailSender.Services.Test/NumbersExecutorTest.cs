using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Yasinovsky.MailSender.Services.Test
{
    public class NumbersExecutorTest : IAsyncLifetime
    {
        private readonly NumbersExecutor _executor = new(".\\NumbersForTest\\", _outFilename);
        private static readonly string _outFilename = ".\\NumbersForTest\\Result.data";

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public void ExecuteAndSaveResults_ResultData_Exists()
        {
            try
            {
                File.Delete(_outFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _executor.ExecuteAndSaveResults();

            Assert.True(File.Exists(_outFilename));
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}