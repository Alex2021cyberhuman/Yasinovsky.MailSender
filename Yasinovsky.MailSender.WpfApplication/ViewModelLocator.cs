using Microsoft.Extensions.DependencyInjection;
using Yasinovsky.MailSender.Data;

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
