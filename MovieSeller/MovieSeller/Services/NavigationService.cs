using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MovieSeller.Core;
using MovieSeller.Extensions;

namespace MovieSeller.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;
        private readonly IServiceProvider _provider;
        private readonly IDictionary<string, Type> _pages;

        public NavigationService(IServiceProvider provider, Frame frame)
        {
            _provider = provider;
            _frame = frame;
        }

        public void Configure(string key, Type type)
        {
#if DEBUG
            // NOTE: Checks if page exists
            var page = _provider.GetService(type);
            if (page is null)
                throw new ArgumentNullException(nameof(type));
#endif
            _pages.Add(key, type);
        }
        
        public Task NavigateToAsync(string key, object context, bool saveHistory = true)
        {
            var page = _provider.GetService(_pages[key]) as Page;
            if (context is not null && page is not null)
                page.DataContext = context;
            if (!saveHistory)
                _frame.RemoveHistory();
            _frame.Navigate(page);
            return Task.CompletedTask;
        }

        public bool CanGoBack => _frame.CanGoBack;

        public Task GoBackAsync()
        {
            _frame.GoBack();
            return Task.CompletedTask;
        }
    }
}
