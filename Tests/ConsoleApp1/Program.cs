using System;
using System.Linq;
using GeneratedCode;
using Yasinovsky.MailSender.Core.Models;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var generatedClass = new GeneratedClass();
            generatedClass.CreatePackage("new.xlsx", Enumerable.Range(0, 25).Select(index =>
                new Recipient
                {
                    Id = index, Address = $"example{index}@gmail.com", Name = $"Recipient#{index}"
                }));
        }
    }
}
