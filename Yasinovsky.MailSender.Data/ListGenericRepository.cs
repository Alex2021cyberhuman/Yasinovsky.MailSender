using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Data;

namespace Yasinovsky.MailSender.Data
{
    public class ListGenericRepository<T> : IRepository<T>
    {
        private readonly Comparison<T> _comparer;

        public ListGenericRepository(Comparison<T> comparer)
        {
            _comparer = comparer;
        }

        public static ICollection<T> Items { get; set; } = new List<T>();

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression)
        {
            await Task.CompletedTask;
            return Items.AsQueryable().Where(expression).ToList();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            await Task.CompletedTask;
            return Items.AsQueryable().FirstOrDefault(expression);
        }

        public async Task<T> AddAsync(T item)
        {
            lock (Items)
            {
                Items.Add(item);
            }

            await Task.CompletedTask;
            return item;
        }

        public async Task<T> UpdateAsync(T item)
        {
            await Task.CompletedTask;
            lock (Items)
            {
                var itemFromList = Items.FirstOrDefault(x => _comparer(x, item) == 0);
                Items.Remove(itemFromList);
                Items.Add(item);
                return item;
            }
        }

        public Task RemoveAsync(T item)
        {
            lock (Items)
            {
                if (!Items.Remove(item))
                    throw new InvalidOperationException("Item not found");
            }

            return Task.CompletedTask;
        }
    }
}