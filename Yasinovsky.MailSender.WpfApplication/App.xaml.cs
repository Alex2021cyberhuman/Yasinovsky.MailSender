using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Data;
using Yasinovsky.MailSender.Services;
using Yasinovsky.MailSender.Services.Wpf;
using Yasinovsky.MailSender.WpfApplication.Views;
using Yasinovsky.MailSender.Core.Extensions;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.WpfApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Exit += App_Exit;
            var builder = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureLogging(ConfigureLogging);
            Host = builder.Build();
        }

        
        public static IHost Host { get; set; }

        private void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddDebug();
        }

        private void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configuration)
        {
            configuration.AddCommandLine(Environment.GetCommandLineArgs())
                .AddJsonFile("appsettings.json", false, true);

        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>();

            services.AddSingleton<IUserDialogService, CustomWindowUserDialogService>();
            services.AddSingleton<IServerUserDialogService, CustomWindowUserDialogService>();
            services.AddSingleton<ISenderUserDialogService, CustomWindowUserDialogService>();
            services.AddSingleton<IRecipientUserDialogService, CustomWindowUserDialogService>();

            services.AddSingleton<IEncryptService, SymmetricEncryptService>( provider =>
            {
                var aes = Aes.Create();
                var key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("2B46E0D1-07A4-4035-840B-9E43815BA839"));
                var iv = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("C95F6D6B-1142-4DE3-9FDA-9DE4647FD04D")).Take(16).ToArray();
                return new SymmetricEncryptService(aes, key, iv);
            });

            services.AddSingleton<IUnitOfWork, JsonFileMailSenderUnitOfWork>(provider =>
            {

                var configuration = provider.GetRequiredService<IConfiguration>();
                var directory = string.IsNullOrWhiteSpace(configuration["JsonDatabase"]) ? new DirectoryInfo(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Yasinovsky.MailSender.JsonDatabase")) : new DirectoryInfo(configuration["JsonDatabase"]);
                if (!directory.Exists)
                    directory.Create();
                var options = new JsonSerializerOptions(JsonSerializerDefaults.General)
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                var unitOfWork = new JsonFileMailSenderUnitOfWork(directory, options);
                return unitOfWork;
            });

            services.AddSingleton<IEmailSendService, MailKitSmtpEmailSendService>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<IEmailSendService>>();
                var service = provider.GetRequiredService<IEncryptService>();
                return new MailKitSmtpEmailSendService(logger, service);
            });
        }


        private async void App_Startup(object sender, StartupEventArgs e)
        {
            await Host.StartAsync();
            var service = Host.Services.GetRequiredService<IEncryptService>();
            await TestData.EncryptPasswords(service);

            using var scope = Host.Services.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            if (bool.Parse(configuration["SeedDatabase"]))
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                if (!unitOfWork.Set<Message>().Any())
                    foreach (var message in TestData.Messages)
                        await unitOfWork.Set<Message>().AddAsync(message);

                if (!unitOfWork.Set<Server>().Any())
                    foreach (var server in TestData.Servers)
                        await unitOfWork.Set<Server>().AddAsync(server);
                if (!unitOfWork.Set<Sender>().Any())
                    foreach (var item in TestData.Senders)
                        await unitOfWork.Set<Sender>().AddAsync(item);

                if (!unitOfWork.Set<Recipient>().Any())
                    foreach (var recipient in TestData.Recipients)
                        await unitOfWork.Set<Recipient>().AddAsync(recipient);

                await unitOfWork.CommitAsync();


                if (!unitOfWork.Set<ScheduleTask>().Any())
                    foreach (var scheduleTask in TestData.ScheduleTasks)
                    {
                        await unitOfWork.Set<ScheduleTask>().AddAsync(scheduleTask);
                    }

                await unitOfWork.CommitAsync();
            }


            var mainWindow = scope.ServiceProvider.GetRequiredService<MainWindow>();
            MainWindow = mainWindow;
            MainWindow.Show();
        }

        private async void App_Exit(object sender, ExitEventArgs e)
        {
            await Host.StopAsync();
            Host.Dispose();
        }

    }
}
