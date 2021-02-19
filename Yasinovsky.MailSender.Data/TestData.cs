using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Data
{
    public class TestData
    {
        private static bool _encrypted = false;

        public static async Task EncryptPasswords(IEncryptService value)
        {
            if (_encrypted)
                throw new InvalidOperationException("Already encrypt");
            foreach (var server in Servers)
            {
                server.Password = await value.EncryptStringAsync(server.Password);
            }

            _encrypted = true;
        }

        public static IList<Server> Servers { get; } = new List<Server>
        {
            new Server
            {
                Id = 1,
                Name = "Яндекс",
                Address = "smpt.yandex.ru",
                Port = 465,
                EnableSsl = true,
                Login = "user@yandex.ru",
                Password = "PassWord",
            },
            new Server
            {
                Id = 2,
                Name = "gMail",
                Address = "smpt.gmail.com",
                Port = 465,
                EnableSsl = true,
                Login = "user@yandex.ru",
                Password = "PassWord",
            },
            new Server
            {
                Id = 3,
                Name = "sendinblue",
                Address = "smtp-relay.sendinblue.com",
                Port = 587,
                EnableSsl = true,
                Login = "yasinoabc@gmail.com",
                Password = "wy8aYUO9vsmC7nbt",
            }
        };

        public static IList<Sender> Senders { get; } = new List<Sender>
        {
            new Sender
            {
                Id = 1,
                Name = "Иванов",
                Address = "ivanov@server.ru",
                Description = "Почта от Иванова"
            },
            new Sender
            {
                Id = 2,
                Name = "Петров",
                Address = "petrov@server.ru",
                Description = "Почта от Петрова"
            },
            new Sender
            {
                Id = 3,
                Name = "Сидоров",
                Address = "sidorov@server.ru",
                Description = "Почта от Сидорова"
            },
        };

        public static IList<Recipient> Recipients { get; } = new List<Recipient>
        {
            new Recipient
            {
                Id = 1,
                Name = "Иванов",
                Address = "ivanov@server.ru",
                Description = "Почта для Иванова"
            },
            new Recipient
            {
                Id = 2,
                Name = "Петров",
                Address = "petrov@server.ru",
                Description = "Почта для Петрова"
            },
            new Recipient
            {
                Id = 3,
                Name = "Сидоров",
                Address = "sidorov@server.ru",
                Description = "Почта для Сидорова"
            },
        };

        public static IList<Message> Messages { get; } = Enumerable
            .Range(1, 10)
            .Select(i => new Message
            {
                Id = i,
                Title = $"Сообщение {i}",
                Body = $"Текст сообщения {i}"
            })
            .ToList();
    }
}