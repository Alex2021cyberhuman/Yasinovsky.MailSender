using System;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Data
{
    public class ListMailSenderUnitOfWork : IUnitOfWork
    {
        private readonly ListGenericRepository<Message> _messages = new ListGenericRepository<Message>((x, y) => x.Id - y.Id);
        private readonly ListGenericRepository<Server> _servers = new ListGenericRepository<Server>((x, y) => x.Id - y.Id);
        private readonly ListGenericRepository<Sender> _senders = new ListGenericRepository<Sender>((x, y) => x.Id - y.Id);
        private readonly ListGenericRepository<Recipient> _recipients = new ListGenericRepository<Recipient>((x, y) => x.Id - y.Id);
        private readonly ListGenericRepository<ScheduleTask> _scheduleTasks = new ListGenericRepository<ScheduleTask>((x, y) => x.Id - y.Id);

        static ListMailSenderUnitOfWork()
        {
            ListGenericRepository<Message>.Items = TestData.Messages;
            ListGenericRepository<Server>.Items = TestData.Servers;
            ListGenericRepository<Sender>.Items = TestData.Senders;
            ListGenericRepository<Recipient>.Items = TestData.Recipients;
            ListGenericRepository<ScheduleTask>.Items = TestData.ScheduleTasks;
        }

        public ListMailSenderUnitOfWork()
        {
        }
        
        public IRepository<T> Set<T>()
        {
            var type = nameof(T);
            return type switch
            {
                "Message" => (IRepository<T>) _messages,
                "Server" => (IRepository<T>) _servers,
                "Sender" => (IRepository<T>) _senders,
                "Recipient" => (IRepository<T>) _recipients,
                "ScheduleTask" => (IRepository<T>) _scheduleTasks,
                _ => throw new InvalidOperationException("Repository not found")
            };
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }
    }
}