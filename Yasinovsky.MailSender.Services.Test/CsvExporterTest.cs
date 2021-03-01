using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Yasinovsky.MailSender.Services.Test
{
    public class CsvExporterTest
    {
        private CsvImporter<Person> _importer;

        public CsvExporterTest()
        {
            _importer = new CsvImporter<Person>(
                new FileStream("persons.csv", FileMode.Open, FileAccess.Read),
                Encoding.UTF8, ",",
                Person.Parse);
        }

        [Fact]
        public void Items_PLinqSumId_Work()
        {
            var sum = _importer.Items.Sum(x => x.Id);
            Assert.True(sum > 0);
        }

        [Fact]
        public void Items_PLinqCount_Work()
        {
            var count = _importer.Items.Count();
            Assert.True(count == 10000);
        }

        [Fact]
        public async Task Convert_ToTxt_Work()
        {
            var exporter = new CsvExporter<Person>(
                new FileStream("persons.txt", FileMode.Truncate, FileAccess.Write),
                Encoding.UTF8,
                " ",
                (person, s) => person.ToString(s),
                ParallelEnumerable.Range(1, 100_000).Select(i => new Person{Id = i, Name = "NameN" + i}));
            
            await exporter.ExecuteAsync();
            exporter.Dispose();

            var importer1 = new CsvImporter<Person>(
                new FileStream("persons.txt", FileMode.Open, FileAccess.Read),
                Encoding.UTF8, " ",
                Person.Parse);
            
            Assert.Equal(100_000, importer1.Items.Count());
        }

        [Theory]
        [MemberData(nameof(Items_PLinqCountId_Work_Data))]
        public void Items_PLinqCountId_Work(Func<Person, bool> predicate)
        {
            var count = _importer.Items.Count(predicate);
            Assert.True(count >= 0);
        }


        public static IEnumerable<object[]> Items_PLinqCountId_Work_Data = new object[][]
        {
            new Func<Person, bool>[] {person => person.Id % 10 == 0},
            new Func<Person, bool>[] {person => person.Name.Length < 10},
            new Func<Person, bool>[] {person => person.Name.Length > 5},
            new Func<Person, bool>[] {person => person.Name.StartsWith('A')}
        };
    }
}