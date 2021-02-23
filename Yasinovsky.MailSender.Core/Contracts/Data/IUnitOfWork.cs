using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Contracts.Data
{
    public interface IUnitOfWork
    {
        IRepository<T> Set<T>();

        Task CommitAsync();
    }
}