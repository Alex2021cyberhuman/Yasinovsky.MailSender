using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using Yasinovsky.MailSender.Data;
using Yasinovsky.MailSender.Services;

namespace Yasinovsky.MailSender.WpfApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private async void App_Startup(object sender, StartupEventArgs e)
        {
            var aes = Aes.Create();
            var service = new SymmetricEncryptService(
                aes, aes.Key, aes.IV);
            await TestData.EncryptPasswords(service);
        }
    }
}
