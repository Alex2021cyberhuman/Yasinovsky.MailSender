using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Yasinovsky.MailSender.Services
{
    public class CsvImporter<T> : IDisposable where T : class
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private readonly string _separator;
        private readonly Func<string, string, T> _importFunction;

        /// <summary>
        /// Read csv in plinq
        /// </summary>
        /// <remarks>
        /// This class is thread-unsafe
        /// </remarks>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="separator"></param>
        /// <param name="importFunction">arg1 - csv string, arg2 - separator</param>
        public CsvImporter(Stream stream, Encoding encoding, string separator, Func<string, string, T> importFunction)
        {
            _stream = stream;
            _encoding = encoding;
            _separator = separator;
            _importFunction = importFunction;
        }

        public ParallelQuery<T> Items => Enumerable.AsParallel();

        public IEnumerable<T> Enumerable
        {
            get
            {
                var reader = new StreamReader(_stream, _encoding);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        yield return _importFunction.Invoke(line, _separator);
                }
            }
        }

        public void Dispose()
        {
            _stream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}