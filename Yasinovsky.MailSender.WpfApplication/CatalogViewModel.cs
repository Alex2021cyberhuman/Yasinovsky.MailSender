using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Core.Models;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.WpfApplication
{
    public class CatalogViewModel : BaseViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServerUserDialogService _serverUserDialog;
        private ObservableCollection<Recipient> _recipients;
        private ObservableCollection<Sender> _senders;
        private ObservableCollection<Server> _servers;

        public CatalogViewModel(IUnitOfWork unitOfWork, IServerUserDialogService serverUserDialog)
        {
            _unitOfWork = unitOfWork;
            _serverUserDialog = serverUserDialog;
            Recipients = new ObservableCollection<Recipient>(_unitOfWork.Set<Recipient>().ToList());
            Senders = new ObservableCollection<Sender>(_unitOfWork.Set<Sender>().ToList());
            Servers = new ObservableCollection<Server>(_unitOfWork.Set<Server>().ToList());
            AddSenderCommand = new WpfCustomCommand(OnAddServer);
            RemoveSenderCommand = new WpfCustomCommand(OnRemoveServer, () => SelectedServer is not null);
            EditSenderCommand = new WpfCustomCommand(OnEditServer, () => SelectedServer is not null);
        }

        public ObservableCollection<Recipient> Recipients
        {
            get => _recipients;
            set => SetProperty(ref _recipients, value);
        }

        public ObservableCollection<Sender> Senders
        {
            get => _senders;
            set => SetProperty(ref _senders, value);
        }

        public ObservableCollection<Server> Servers
        {
            get => _servers;
            set => SetProperty(ref _servers, value);
        }

        public Server SelectedServer { get; set; }

        public ICommand AddSenderCommand { get; }

        public ICommand RemoveSenderCommand { get;}

        public ICommand EditSenderCommand { get; }

        private async void OnAddServer()
        {
            var server = await _serverUserDialog.OpenCreateDialogAsync();
            if (server is null)
                return;

            server = await _unitOfWork.Set<Server>()
                .AddAsync(server);
            Servers.Add(server);
            await _unitOfWork.CommitAsync();
        }

        private async void OnRemoveServer()
        {
            if (SelectedServer is null)
                return;
            Servers.Remove(SelectedServer);
            await _unitOfWork.Set<Server>()
                .RemoveAsync(SelectedServer);
            await _unitOfWork
                .CommitAsync();
        }
        private async void OnEditServer()
        {
            Servers.Remove(SelectedServer);
            var server = await _serverUserDialog.OpenEditDialogAsync(SelectedServer);
            if (server is null)
                throw new NotImplementedException();

            server = await _unitOfWork.Set<Server>()
                .UpdateAsync(server);
            Servers.Add(server);
            await _unitOfWork.CommitAsync();
        }

    }
}