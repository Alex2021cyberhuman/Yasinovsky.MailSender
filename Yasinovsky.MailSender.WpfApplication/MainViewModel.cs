using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.WpfApplication
{
    public class MainViewModel : BaseViewModel
    {
        private CatalogViewModel _catalogViewModel;

        public CatalogViewModel CatalogViewModel
        {
            get => _catalogViewModel;
            set => SetProperty(ref _catalogViewModel, value);
        }

        public MainViewModel(CatalogViewModel catalogViewModel)
        {
            _catalogViewModel = catalogViewModel;
        }
    }
}
