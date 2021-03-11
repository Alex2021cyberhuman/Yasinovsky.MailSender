using System;
using System.Threading.Tasks;

namespace MovieSeller.Core.Services
{
    public interface IDialogNavigationService
    {
        void Configure(string key, Type type);

        Task<bool> ShowDialogAsync(string key, object context = null);
    }
}