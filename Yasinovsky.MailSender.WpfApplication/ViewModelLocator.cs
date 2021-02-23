using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Security.Certificates;
using Yasinovsky.MailSender.Data;
using Yasinovsky.MailSender.Services;
using Yasinovsky.MailSender.Services.Wpf;

namespace Yasinovsky.MailSender.WpfApplication
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            
        }

        public MainViewModel MainViewModel => App.Host.Services.GetService<MainViewModel>() ??
                                              new MainViewModel(new ListMailSenderUnitOfWork(), null, null, null, null,
                                                  null, null);
    }
}
