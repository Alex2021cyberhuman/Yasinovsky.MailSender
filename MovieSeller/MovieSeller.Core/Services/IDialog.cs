using System.Threading.Tasks;

namespace MovieSeller.Core.Services
{
    public interface IDialog
    {
        Task<bool> ShowAsync(object context = null);
    }
}