using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Contracts.Data
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> expression);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T item);

        Task<T> UpdateAsync(T item);

        Task RemoveAsync(T item);
    }
}
