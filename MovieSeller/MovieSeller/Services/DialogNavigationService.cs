using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MovieSeller.Core.Services;

namespace MovieSeller.Services
{
    public class DialogNavigationService : IDialogNavigationService
    {
        private readonly IServiceProvider _provider;
        private readonly IDictionary<string, Type> _dialogs = new Dictionary<string, Type>();

        public DialogNavigationService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Configure(string key, Type type)
        {
#if DEBUG
            // NOTE: Checks if page exists
            var page = _provider.GetService(type);
            if (page is null)
                throw new ArgumentNullException(nameof(type));
            if (page is not Window && page is not IDialog)
                throw new NotImplementedException();
#endif
            _dialogs.Add(key, type);
        }

        public async Task<bool> ShowDialogAsync(string key, object context)
        {
            var mainWindow =
                Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x.IsActive) 
                ?? Application.Current.Windows.Cast<Window>().First();
            var requiredService = _provider.GetRequiredService(_dialogs[key]);
            if (requiredService is IDialog dialog)
                return await dialog.ShowAsync(context);
            if (requiredService is Window window)
            {
                if (context is not null)
                    window.DataContext = context;
                window.Owner = mainWindow;
                var result = window.ShowDialog();
                return result ?? false;
            }

            throw new NotImplementedException();
        }
    }
}