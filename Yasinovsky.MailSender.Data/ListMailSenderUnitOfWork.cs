using System;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Data
{
    public class ListMailSenderUnitOfWork : IUnitOfWork
    {
        private readonly ListGenericRepository<Message> _messages;
        private readonly ListGenericRepository<Server> _servers;
        private readonly ListGenericRepository<Sender> _senders;
        private readonly ListGenericRepository<Recipient> _recipients;
        private readonly ListGenericRepository<ScheduleTask> _scheduleTasks;

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
            _messages = new((x, y) => x.Id - y.Id);
            _servers = new((x, y) => x.Id - y.Id);
            _senders = new((x, y) => x.Id - y.Id);
            _recipients = new((x, y) => x.Id - y.Id);
            _scheduleTasks = new((x, y) => x.Id - y.Id);
        }
        
        public IRepository<T> Set<T>()
        {
            var type = typeof(T);
            if (type == typeof(Message))
                return (IRepository<T>) _messages;
            else if (type == typeof(Server))
                return (IRepository<T>) _servers;
            else if (type == typeof(Sender))
                return (IRepository<T>) _senders;
            else if (type == typeof(Recipient))
                return (IRepository<T>) _recipients;
            else if (type == typeof(ScheduleTask))
                return (IRepository<T>) _scheduleTasks;
            else
                throw new InvalidOperationException("Repository not found");
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }
    }
}