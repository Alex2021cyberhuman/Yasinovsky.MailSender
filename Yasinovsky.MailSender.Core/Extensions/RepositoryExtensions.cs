using System.Collections.Generic;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Core.Extensions
{
    public static class RepositoryExtensions
    {
        public static Task<IEnumerable<T>> GetAllAsync<T>(this IRepository<T> source) =>
            source.GetWhereAsync(x => true);

        public static Task<T> FindById<T>(this IRepository<T> source, int id) where T : IHasId =>
            source.FirstOrDefaultAsync(x => x.Id == id);
    }
}