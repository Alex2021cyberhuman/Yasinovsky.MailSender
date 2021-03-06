using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Yasinovsky.MailSender.Services.Test
{
    public class NumbersExecutorTest : IAsyncLifetime
    {
        private readonly NumbersExecutor _executor;
        private readonly string _outFilename;
        private readonly string _directoryName;

        public NumbersExecutorTest()
        {
            _directoryName = ".\\NumbersForTest\\";
            _outFilename = ".\\NumbersForTest\\Result.data";
            _executor = new(_directoryName, _outFilename);
        }

        
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public async Task ExecuteAndSaveResults_ResultData_ExistsAsync()
        {
            int count = 10000;
            int min = 1;
            int max = 100;
            await _executor.GenerateNumbersInDirectoryAsync(count, min, max);
            if (File.Exists(_outFilename))
                File.Delete(_outFilename);

            _executor.ExecuteAndSaveResults();

            Assert.True(File.Exists(_outFilename));
            Assert.True(File.Exists(_outFilename) && (await File.ReadAllLinesAsync(_outFilename)).Length == count);
        }

        public async Task DisposeAsync()
        {
            Directory.Delete(_directoryName, true);
        }
    }
}