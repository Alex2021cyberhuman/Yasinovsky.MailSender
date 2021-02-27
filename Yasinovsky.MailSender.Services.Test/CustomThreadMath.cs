using System;
using System.Linq;
using System.Threading;

namespace Yasinovsky.MailSender.Services.Test
{
    public class CustomThreadMath
    {
        public static long GetFactorialAsync(long n)
        {
            if (n == 0 || n == 1)
                return 1;
            if (n < 0)
                throw new ArgumentException("Invalid n");
            var partialResults = new long[Environment.ProcessorCount];
            var threads = new Thread[Environment.ProcessorCount];
            for (var threadIndex = 0; threadIndex < Environment.ProcessorCount; threadIndex++)
            {
                var index = threadIndex;
                var thread = new Thread(() =>
                {
                    var result = 1L;
                    for (var i = index + 1; i <= n; i += Environment.ProcessorCount)
                        result *= i;
                    partialResults[index] = result;
                });
                thread.Start();
                threads[threadIndex] = thread;
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            long finalResult = 1;

            foreach (var value in partialResults)
            {
                finalResult *= value;
            }

            return finalResult;
        }


        public static long GetSumOfDigits(int n)
        {
            if (n == 0 || n == 1)
                return n;
            if (n < 0)
                throw new ArgumentException("Invalid n");
            var partialResults = new long[Environment.ProcessorCount];
            var threads = new Thread[Environment.ProcessorCount];
            for (var threadIndex = 0; threadIndex < Environment.ProcessorCount; threadIndex++)
            {
                var index = threadIndex;
                var thread = new Thread(() =>
                {
                    var result = 0;
                    for (var i = index; i <= n; i += Environment.ProcessorCount)
                        result += i;
                    partialResults[index] = result;
                });
                thread.Start();
                threads[threadIndex] = thread;
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            long finalResult = 0;

            foreach (var value in partialResults)
            {
                finalResult += value;
            }

            return finalResult;
        }

        public static long GetSumOfDigitsParallelRange(int n)
        {
            if (n == 0 || n == 1)
                return n;
            if (n < 0)
                throw new ArgumentException("Invalid n");
            var partialResults = new long[Environment.ProcessorCount];
            return ParallelEnumerable.Range(1, n).Sum();
        }
    }
}