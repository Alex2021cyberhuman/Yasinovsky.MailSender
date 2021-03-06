using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Data
{
    public class JsonFileMailSenderUnitOfWork : IUnitOfWork
    {
        private readonly JsonFileGenericRepository<Message> _messages;
        private readonly JsonFileGenericRepository<Server> _servers;
        private readonly JsonFileGenericRepository<Sender> _senders;
        private readonly JsonFileGenericRepository<Recipient> _recipients;
        // WARN: USING THIS REPOSITORY MAKES TASK SCHEDULING IMPOSSIBLE
        private readonly JsonFileGenericRepository<ScheduleTask> _scheduleTasks;

        public JsonFileMailSenderUnitOfWork(DirectoryInfo directory, JsonSerializerOptions options)
        {
            if (!directory.Exists)
                throw new DirectoryNotFoundException(nameof(directory));
            _messages = new(options, new FileInfo(Path.Combine(directory.FullName, nameof(Message) + "Set.json")));
            _servers = new(options, new FileInfo(Path.Combine(directory.FullName, nameof(Server) + "Set.json")));
            _senders = new(options, new FileInfo(Path.Combine(directory.FullName, nameof(Sender) + "Set.json")));
            _recipients = new(options, new FileInfo(Path.Combine(directory.FullName, nameof(Recipient) + "Set.json")));
            _scheduleTasks = new(options, new FileInfo(Path.Combine(directory.FullName, nameof(ScheduleTask) + "Set.json")));
        }

        public IRepository<T> Set<T>()
        {
            var type = typeof(T);
            if (type == typeof(Message))
                return (IRepository<T>) _messages;
            if (type == typeof(Server))
                return (IRepository<T>)_servers;
            if (type == typeof(Sender))
                return (IRepository<T>)_senders;
            if (type == typeof(Recipient))
                return (IRepository<T>)_recipients;
            if (type == typeof(ScheduleTask))
                return (IRepository<T>)_scheduleTasks;
            throw new InvalidOperationException("Repository not found");
        }

        public async Task CommitAsync()
        {
            await _messages.CommitAsync();
            await _servers.CommitAsync();
            await _senders.CommitAsync();
            await _recipients.CommitAsync();
            await  _scheduleTasks.CommitAsync();
        }
    }
}