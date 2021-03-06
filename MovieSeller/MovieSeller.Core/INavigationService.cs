using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSeller.Core
{
    public interface INavigationService
    {
        void Configure(string key, Type type);
        
        Task NavigateToAsync(string key, object context, bool saveHistory = true);

        bool CanGoBack { get; }

        Task GoBackAsync();
    }
}
