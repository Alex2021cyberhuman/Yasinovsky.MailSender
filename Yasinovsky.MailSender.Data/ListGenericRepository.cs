using System;
using System.Collections;
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

        private static IQueryable<T> Queryable => Items.AsQueryable();

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) Items).GetEnumerator();

        public Type ElementType => Queryable.ElementType;

        public Expression Expression => Queryable.Expression;

        public IQueryProvider Provider => Queryable.Provider;

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