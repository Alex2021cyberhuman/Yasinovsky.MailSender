using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Models;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Data
{
    public class DbContextUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly DbContextRepository<Message> _messages;
        private readonly DbContextRepository<Recipient> _recipiens;
        private readonly DbContextRepository<ScheduleTask> _scheduleTasks;
        private readonly DbContextRepository<Sender> _senders;
        private readonly DbContextRepository<Server> _servers;

        private readonly IDictionary<Type, object> _repositories;

        public DbContextUnitOfWork(DbContext context)
        {
            _context = context;
            _messages = new(_context);
            _servers = new(_context);
            _senders= new(_context);
            _scheduleTasks= new(_context);
            _recipiens= new(_context);
            _repositories = new Dictionary<Type, object>
            {
                {typeof(Message), _messages},
                {typeof(Server), _servers},
                {typeof(ScheduleTask), _scheduleTasks},
                {typeof(Recipient), _recipiens},
                {typeof(Sender), _senders}
            };
        }

        public IRepository<T> Set<T>() where T : class, IHasId => (IRepository<T>) ( _repositories.ContainsKey(typeof(T)) ? _repositories[typeof(T)] : _repositories[typeof(T)] = new DbContextRepository<T>(_context));

        public Task CommitAsync() => _context.SaveChangesAsync();
    }
}
