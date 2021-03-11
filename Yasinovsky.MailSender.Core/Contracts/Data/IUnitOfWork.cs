using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Core.Contracts.Data
{
    public interface IUnitOfWork
    {
        IRepository<T> Set<T>() where T : class, IHasId;

        Task CommitAsync();
    }
}