using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Services
{
    public class NumbersExecutor
    {
        private readonly string _directoryName;
        private readonly string _outFilename; 
        private  static Regex _numberRegex = new Regex(@"^([1|2]) (.*) (.*)$", RegexOptions.Compiled);

        public NumbersExecutor(string directoryName, string outFilename)
        {
            _directoryName = directoryName;
            if (!Directory.Exists(_directoryName))
                Directory.CreateDirectory(_directoryName);
            _outFilename = outFilename;
        }

        public  Task GenerateNumbersInDirectoryAsync(IEnumerable<(string, int, decimal, decimal)> inputs)
        {

            //var factory = new TaskFactory();
            //// NOTE: will throw if it is not numbers
            //var writingTasks = new List<Task>(32);
            var task = Task.Run(() =>
            {
                var writeRange = Partitioner.Create(
                    inputs);
                writeRange.AsParallel()
                    .ForAll(tuple => File.WriteAllText(
                        Path.Combine(_directoryName, tuple.Item1),
                        $"{tuple.Item2} {tuple.Item3} {tuple.Item4}"));
            });
            return task;

            //foreach (var tuple in inputs)
            //{
            //    writingTasks.Add(
            //        factory.StartNew(() => File.WriteAllText(
            //            Path.Combine(_directoryName, tuple.Item1),
            //            $"{tuple.Item2} {tuple.Item3} {tuple.Item4}"), 
            //            TaskCreationOptions.LongRunning));
            //}

            //await Task.WhenAll(writingTasks);
        }

        public Task GenerateNumbersInDirectoryAsync(int count = 50000, int min = 1, int max = 100)
        {
            var random = new Random();
            return GenerateNumbersInDirectoryAsync(
                Enumerable
                .Range(0, count)
                .Select(i => new ValueTuple<string, int, decimal, decimal>
                    ($"number-data-{random.Next(Int32.MinValue, Int32.MaxValue)}.txt",  i % 2,
                    random.Next(min, max) + random.Next(1, 99) * 0.01m,
                    random.Next(min, max) + random.Next(1, 99) * 0.01m))
                .ToList().AsEnumerable()
            );
        }

        public void ExecuteAndSaveResults()
        {
            var fileNames = Directory.EnumerateFiles(_directoryName);
            var fileRange = Partitioner.Create(fileNames);
            //var factory = new TaskFactory<decimal?>();
            //// NOTE: will throw if it is not numbers
            //var readingTasks = new List<Task<decimal?>>(32);
            //foreach (var fileName in fileNames)
            //{
            //    readingTasks.Add(
            //        factory.StartNew(() => CalculateText(fileName), TaskCreationOptions.LongRunning));
            //}
            var writerStringBuilder = new StringBuilder(1024);

            writerStringBuilder.AppendJoin("\n",
                fileRange.AsParallel()
                    .Select(fileName => File.ReadAllTextAsync(fileName))
                    .Select(task => task.Result)
                    .Select(CalculateText)
                    .Where(number => number.HasValue)
                    .Select(number => number.ToString()));

            var writer = new StreamWriter(_outFilename);
            var writeThread = new Thread( () => writer.Write(writerStringBuilder));
            writeThread.Start();
            writeThread.Join();

            writer.Dispose();
            
        }

        private static decimal? CalculateText(string number)
        {
            var match = _numberRegex.Match(number);
            if (!match.Success)
                return null;
            var type = match.Groups[1].Value;
            var a = decimal.Parse(match.Groups[2].Value);
            var b = decimal.Parse(match.Groups[3].Value);
            if (type == "1")
                return a * b;
            if (type == "2" && b != 0)
                return a / b;
            return null;
        }
    }
}
