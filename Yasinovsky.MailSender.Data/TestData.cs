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

        public static Server TestServer => Servers[0];
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

        public static IList<ScheduleTask> ScheduleTasks { get; } = Messages
            .Select(message => new ScheduleTask
            {
                Id = message.Id,
                Message = message,
                MessageId = message.Id,
                Sender = Senders[0],
                SenderId = Senders[0].Id,
                Server = Servers[0],
                ServerId = Servers[0].Id,
                Recipients = Recipients,
                Created = DateTime.Now.AddDays(-1),
                Scheduled = DateTime.Now.AddDays(1)
            }).ToList();

    }
}