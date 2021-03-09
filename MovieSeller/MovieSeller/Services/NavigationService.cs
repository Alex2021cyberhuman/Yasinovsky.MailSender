using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MovieSeller.Core;
using MovieSeller.Core.Services;
using MovieSeller.Extensions;

namespace MovieSeller.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;
        private readonly IServiceProvider _provider;
        private readonly IDictionary<string, Type> _pages = new Dictionary<string, Type>();
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
            if (page is not Page)
                throw new NotImplementedException();
#endif
            _pages.Add(key, type);
        }
        
        public Task NavigateToAsync(string key, object context = null, bool saveHistory = true)
        {
            return _frame.Dispatcher.InvokeAsync(() =>
            {
                var requiredService = _provider.GetRequiredService(_pages[key]);
                if (context is not null && requiredService is FrameworkElement frameworkElement)
                    frameworkElement.DataContext = context;
                if (!saveHistory)
                    _frame.RemoveHistory();
                _frame.Navigate(requiredService);
            }).Task;
        }

        public bool CanGoBack => _frame.CanGoBack;

        public Task GoBackAsync()
        {
            _frame.GoBack();
            return Task.CompletedTask;
        }
    }
}
