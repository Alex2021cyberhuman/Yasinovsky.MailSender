using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Services
{
    public class CsvExporter<T> : IDisposable where T : class
    {
        private readonly Stream _stream;
        private readonly Encoding _encoding;
        private readonly string _separator;
        private readonly Func<T, string, string> _exportFunction;
        private readonly ParallelQuery<T> _items;

        /// <summary>
        /// Save plinq to csv
        /// </summary>
        /// <remarks>
        /// This class is thread-unsafe
        /// </remarks>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="separator"></param>
        /// <param name="exportFunction">arg1 - item, arg2 - separator</param>
        /// <param name="items"></param>
        public CsvExporter(Stream stream, Encoding encoding, string separator, Func<T, string, string> exportFunction, ParallelQuery<T> items)
        {
            _stream = stream;
            _encoding = encoding;
            _separator = separator;
            _exportFunction = exportFunction;
            _items = items;
        }

        public async Task ExecuteAsync()
        {
            var stringItems = _items.Select(x =>  _exportFunction(x, _separator));
            var processorsCount = Environment.ProcessorCount;
            var stringBuilders = stringItems
                .Select((s, i) => (value: s, index: i))
                .GroupBy(tuple => tuple.index % processorsCount)
                .Select(x =>
                {
                    var builder = new StringBuilder();
                    foreach (var item in x)
                    {
                        builder.AppendLine(item.value);
                    }

                    return builder;
                });
            var writer = new StreamWriter(_stream, _encoding);
            var array = stringBuilders.ToArray();
            await writer.WriteAsync(string.Join("", array.Select(x => x.ToString())));
        }

        public void Dispose()
        {
            _stream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}