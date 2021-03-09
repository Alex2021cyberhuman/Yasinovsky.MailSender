using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MovieSeller.Core.Data;
using MovieSeller.Core.Services;
using MovieSeller.Data;
using MovieSeller.Data.DataManagers;
using MovieSeller.Services;
using MovieSeller.ViewModels;
using MovieSeller.Views.Pages;
using MovieSeller.Views.Windows;

namespace MovieSeller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost _host;
        public static IHost Host => GetHost();

        private static IHost GetHost()
        {
            if (_host is not null)
                return _host;
            _host = CreateHostBuilder().Build();
            _host.Start();
            return _host;
        }

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {

            var scope = Host.Services.CreateScope();
            var mainWindow = scope.ServiceProvider.GetRequiredService<MainWindow>();
            MainWindow = mainWindow;
            MainWindow.Show();
            var navigationService = scope.ServiceProvider.GetRequiredService<INavigationService>();
            await navigationService.NavigateToAsync(nameof(MovieSessionsViewModel));
        }

        private static IHostBuilder CreateHostBuilder()
        {
            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureServices(ConfigureServices);

            hostBuilder.ConfigureLogging(ConfigureLogging);

            hostBuilder.ConfigureAppConfiguration(ConfigureAppConfiguration);

            return hostBuilder;
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddCommandLine(Environment.GetCommandLineArgs());
            configurationBuilder.AddEnvironmentVariables();
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.TimestampFormat = "'['hh':'mm':'ss']'";
                options.ColorBehavior = LoggerColorBehavior.Enabled;
                options.UseUtcTimestamp = false;
            });
            loggingBuilder.AddDebug();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Add Views
            services.AddSingleton<MainWindow>();
            services.AddTransient<EditMovieWindow>();
            services.AddTransient<BuyBookingWindow>();

            services.AddTransient<MovieSessionsPage>();
            services.AddTransient<CreateNewMovieSessionPage>();

            // Add ViewModels
            services.AddTransient<MovieSessionsPage>();

            // Add Services
            services.AddScoped<IBookingDataManager, DbBookingDataManager>();
            services.AddScoped<IMovieDataManager, DbMovieDataManager>();
            services.AddScoped<IMovieSessionDataManager, DbMovieSessionDataManager>();

            services.AddDbContext<MovieSellerDbContext>(options =>
                options.EnableDetailedErrors().EnableSensitiveDataLogging().UseSqlite("Data Source=.\\database.db"));


            services.AddSingleton(provider => provider.GetRequiredService<MainWindow>().MainWindowFrame);

            services.AddSingleton<NavigationService>();
            services.AddSingleton<INavigationService>(provider =>
            {
                var navigationService = provider.GetRequiredService<NavigationService>();
                navigationService.Configure(nameof(MovieSessionsViewModel), typeof(MovieSessionsPage));
                navigationService.Configure(nameof(CreateNewMovieSessionViewModel), typeof(CreateNewMovieSessionPage));
                return navigationService;
            });

            services.AddSingleton<DialogNavigationService>();
            services.AddSingleton<IDialogNavigationService>(provider =>
            {
                var navigationService = provider.GetRequiredService<DialogNavigationService>();
                navigationService.Configure(nameof(EditMovieWindow), typeof(EditMovieWindow));
                navigationService.Configure(nameof(BuyBookingWindow), typeof(BuyBookingWindow));
                return navigationService;
            });
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Host.StopAsync();

            Host.Dispose();
        }
    }
}
