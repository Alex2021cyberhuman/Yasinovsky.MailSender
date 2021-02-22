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
        private readonly IEncryptService _encryptService;
        private ObservableCollection<Recipient> _recipients;
        private ObservableCollection<Sender> _senders;
        private ObservableCollection<Server> _servers;
        private Server _selectedServer;

        public CatalogViewModel(IUnitOfWork unitOfWork, IServerUserDialogService serverUserDialog, IEncryptService encryptService)
        {
            _unitOfWork = unitOfWork;
            _serverUserDialog = serverUserDialog;
            _encryptService = encryptService;
            Recipients = new ObservableCollection<Recipient>(_unitOfWork.Set<Recipient>().ToList());
            Senders = new ObservableCollection<Sender>(_unitOfWork.Set<Sender>().ToList());
            Servers = new ObservableCollection<Server>(_unitOfWork.Set<Server>().ToList());
            SelectedServer = Servers.FirstOrDefault();
            AddServerCommand = new WpfCustomCommand(OnAddServer);
            RemoveServerCommand = new WpfCustomCommand(OnRemoveServer, () => SelectedServer is not null);
            EditServerCommand = new WpfCustomCommand(OnEditServer, () => SelectedServer is not null);
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

        public Server SelectedServer
        {
            get => _selectedServer;
            set => SetProperty(ref _selectedServer, value);
        }


        public ICommand AddServerCommand { get; }

        public ICommand RemoveServerCommand { get;}

        public ICommand EditServerCommand { get; }

        private async void OnAddServer()
        {
            var server = await _serverUserDialog.OpenCreateDialogAsync();
            if (server is null)
                return;
            server.Password = await _encryptService.EncryptStringAsync(server.Password);
            server = await _unitOfWork.Set<Server>()
                .AddAsync(server);
            Servers.Add(server);
            await _unitOfWork.CommitAsync();
            SelectedServer = Servers.LastOrDefault();
        }

        private async void OnRemoveServer()
        {
            if (SelectedServer is null)
                return;
            await _unitOfWork.Set<Server>()
                .RemoveAsync(SelectedServer);
            Servers.Remove(SelectedServer);
            await _unitOfWork
                .CommitAsync();
            SelectedServer = Servers.FirstOrDefault();
        }
        private async void OnEditServer()
        {
            if (SelectedServer is null)
                return;
            var server = SelectedServer;
            Servers.Remove(SelectedServer);
            var password = server.Password;
            server.Password = string.Empty;
            server = await _serverUserDialog.OpenEditDialogAsync(server);
            if (server is null)
                throw new NotImplementedException();
            if (string.IsNullOrWhiteSpace(server.Password))
                server.Password = password;
            else
                server.Password = await _encryptService.EncryptStringAsync(server.Password);
            server = await _unitOfWork.Set<Server>()
                .UpdateAsync(server);
            Servers.Add(server);
            await _unitOfWork.CommitAsync();
            SelectedServer = server;
        }

    }
}