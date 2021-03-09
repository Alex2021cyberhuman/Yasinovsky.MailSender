using System;
using System.Threading.Tasks;

namespace MovieSeller.Core.Services
{
    public interface INavigationService
    {
        void Configure(string key, Type type);
        
        Task NavigateToAsync(string key, object context = null, bool saveHistory = true);

        bool CanGoBack { get; }

        Task GoBackAsync();
    }
}
