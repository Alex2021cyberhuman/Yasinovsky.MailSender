using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yasinovsky.MailSender.Core.Models.Base;
using Yasinovsky.MailSender.Services.Wpf.Commands;

namespace MovieSeller.ViewModels
{
    public class BuyBookingViewModel : BaseViewModel
    {
        private int _count;
        private int _maxCount;

        public BuyBookingViewModel() : this(0, 0)
        {
            
        }

        public BuyBookingViewModel(int count, int maxCount)
        {
            _count = count;
            _maxCount = maxCount;
            CancelCommand = new CustomCommand(() => OnDialogClose(false));
            SaveCommand = new WpfCustomCommand(() => OnDialogClose(true), () => Count > 0);
        }

        public ICommand CancelCommand { get; set; }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }


        public int MaxCount
        {
            get => _maxCount;
            set => SetProperty(ref _maxCount, value);
        }

        public ICommand SaveCommand { get; }

        public event EventHandler<bool> DialogClose;

        protected virtual void OnDialogClose(bool e)
        {
            DialogClose?.Invoke(this, e);
        }
    }
}
