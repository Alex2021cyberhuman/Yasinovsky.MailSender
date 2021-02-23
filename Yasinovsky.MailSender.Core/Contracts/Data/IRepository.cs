using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Contracts.Data
{
    public interface IRepository<T> : IQueryable<T>
    {
        Task<T> AddAsync(T item);

        Task<T> UpdateAsync(T item);

        Task RemoveAsync(T item);
    }
}
