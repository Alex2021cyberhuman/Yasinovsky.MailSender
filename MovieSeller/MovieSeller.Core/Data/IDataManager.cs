using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MovieSeller.Core.Models.Base;

namespace MovieSeller.Core.Data
{
    public interface IDataManager<T, TKey> where T : class, IHasKey<TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T item);

        Task<T> UpdateAsync(T item);

        Task RemoveAsync(T item);
    }
}
