using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MovieSeller.Core.Models.Domain;
using Yasinovsky.MailSender.Core.Models.Base;
using Yasinovsky.MailSender.Services.Wpf.Commands;

namespace MovieSeller.ViewModels
{
    public class ConfirmDeleteViewModel : BaseViewModel
    {
        private MovieSession _movieSession;

        public ConfirmDeleteViewModel()
        {
            CancelCommand = new CustomCommand(() => OnDialogClose(false));
            ConfirmCommand = new WpfCustomCommand(() => OnDialogClose(true));
        }

        public MovieSession MovieSession
        {
            get => _movieSession;
            set => SetProperty(ref _movieSession, value);
        }

        public ICommand ConfirmCommand { get; }

        public ICommand CancelCommand { get;}

        public event EventHandler<bool> DialogClose;

        protected virtual void OnDialogClose(bool e)
        {
            DialogClose?.Invoke(this, e);
        }
    }
}
