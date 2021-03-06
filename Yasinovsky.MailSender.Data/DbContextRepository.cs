using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Data
{
    public class DbContextRepository<T> : IRepository<T> where T : class, IHasId
    {
        protected readonly DbContext Context;

        public DbContextRepository(DbContext context)
        {
            Context = context;
        }


        public virtual async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression)
        {
            return (await Context.Set<T>().Where(expression).AsNoTracking().ToListAsync()).AsEnumerable();
        }

        public virtual Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public virtual async Task<T> AddAsync(T item)
        {
            var entry = await Context.Set<T>().AddAsync(item);
            return entry.Entity;
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            await Task.CompletedTask;
            var entry = Context.Set<T>().Update(item);
            return entry.Entity;
        }

        public virtual Task RemoveAsync(T item)
        {
            Context.Set<T>().Remove(item);
            return Task.CompletedTask;
        }
    }
}
