using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Contracts.Services;
using Yasinovsky.MailSender.Core.Enums;
using Yasinovsky.MailSender.Core.Models;
using Yasinovsky.MailSender.Core.Models.Base;
using Yasinovsky.MailSender.WpfApplication.Models.Base;

namespace Yasinovsky.MailSender.WpfApplication
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServerUserDialogService _serverUserDialog;
        private readonly IEncryptService _encryptService;
        private readonly ISenderUserDialogService _senderUserDialog;
        private readonly IRecipientUserDialogService _recipientUserDialog;
        private readonly IEmailSendService _emailSendService;
        private readonly IUserDialogService _userDialogService;

        private ObservableCollection<Recipient> _recipients;
        private ObservableCollection<Sender> _senders;
        private ObservableCollection<Server> _servers;
        private Server _selectedServer;
        private Sender _selectedSender;
        private Recipient _selectedRecipient;
        private DateTime _selectedDateTime;
        private Message _selectedMessage;
        private string _messageBody;
        private string _messageTitle;
        private ObservableCollection<ScheduleTask> _scheduleTasks;
        private ObservableCollection<Message> _messages;
        private ObservableCollection<Recipient> _selectedRecipients;

        public MainViewModel(IUnitOfWork unitOfWork,
            IServerUserDialogService serverUserDialog,
            IEncryptService encryptService,
            ISenderUserDialogService senderUserDialog,
            IRecipientUserDialogService recipientUserDialog,
            IUserDialogService userDialogService, 
            IEmailSendService emailSendService)

        {
            _unitOfWork = unitOfWork;
            _serverUserDialog = serverUserDialog;
            _encryptService = encryptService;
            _senderUserDialog = senderUserDialog;
            _recipientUserDialog = recipientUserDialog;
            _userDialogService = userDialogService;
            _emailSendService = emailSendService;

            SendCommand = new WpfCustomCommand(OnSend, () =>
                SelectedSender != null
                && SelectedServer != null
                && SelectedRecipient != null
                && SelectedMessage != null);

            LoadCommand = new CustomCommand(OnLoad);

            AddServerCommand = new WpfCustomCommand(OnAddServer);
            RemoveServerCommand = new WpfCustomCommand(OnRemoveServer,
                () => SelectedServer is not null);
            EditServerCommand = new WpfCustomCommand(OnEditServer,
                () => SelectedServer is not null);

            AddSenderCommand = new WpfCustomCommand(OnAddSender);
            RemoveSenderCommand = new WpfCustomCommand(OnRemoveSender,
                () => SelectedSender is not null);
            EditSenderCommand = new WpfCustomCommand(OnEditSender,
                () => SelectedSender is not null);

            AddRecipientCommand = new WpfCustomCommand(OnAddRecipient);
            RemoveRecipientCommand = new WpfCustomCommand(OnRemoveRecipient,
                () => SelectedRecipient is not null);
            EditRecipientCommand = new WpfCustomCommand(OnEditRecipient,
                () => SelectedRecipient is not null);

            AddMessageCommand = new WpfCustomCommand(OnAddMessage, () =>
                !string.IsNullOrWhiteSpace(MessageTitle)
                && !string.IsNullOrWhiteSpace(MessageBody));

            ConfirmEditMessageCommand = new WpfCustomCommand(OnConfirmEditMessage, () =>
                SelectedMessage is not null
                && !string.IsNullOrWhiteSpace(MessageTitle)
                && !string.IsNullOrWhiteSpace(MessageBody));

            RemoveMessageCommand = new WpfCustomCommand(OnRemoveMessage, () =>
                SelectedMessage is not null);

            SelectedMessageChangedCommand = new WpfCustomCommand(() =>
            {
                MessageTitle = SelectedMessage.Title;
                MessageBody = SelectedMessage.Body;
            }, () => SelectedMessage is not null);
        }

        

        #region Messages


        public ICommand AddMessageCommand { get; }

        public ICommand ConfirmEditMessageCommand { get; }

        public ICommand RemoveMessageCommand { get; }

        public ICommand SelectedMessageChangedCommand { get; }
        
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        public Message SelectedMessage
        {
            get => _selectedMessage;
            set => SetProperty(ref _selectedMessage, value);
        }


        public string MessageTitle
        {
            get => _messageTitle;
            set => SetProperty(ref _messageTitle, value);
        }


        public string MessageBody
        {
            get => _messageBody;
            set => SetProperty(ref _messageBody, value);
        }

        private async void OnAddMessage()
        {
            var message = new Message
            {
                Title = MessageTitle,
                Body = MessageBody
            };
            message = await _unitOfWork.Set<Message>()
                .AddAsync(message);
            Messages.Add(message);
            await _unitOfWork.CommitAsync();
            SelectedMessage = message;
        }

        private async void OnConfirmEditMessage()
        {
            var message = SelectedMessage;
            var index = Messages.IndexOf(message);
            Messages.Remove(message);
            message.Title = MessageTitle;
            message.Body = MessageBody;
            message = await _unitOfWork.Set<Message>()
                .UpdateAsync(message);
            await _unitOfWork.CommitAsync();
            Messages.Insert(index, message);
        }

        private async void OnRemoveMessage()
        {
            var message = SelectedMessage;
            Messages.Remove(message);
            await _unitOfWork.Set<Message>()
                .RemoveAsync(message);
            await _unitOfWork.CommitAsync();
        }

        #endregion
        
        #region Load

        public ICommand LoadCommand { get; }

        private async void OnLoad()
        {
            await _unitOfWork.CommitAsync();
            Recipients = new ObservableCollection<Recipient>(_unitOfWork.Set<Recipient>()
                .ToList());
            SelectedRecipient = Recipients.FirstOrDefault();
            Senders = new ObservableCollection<Sender>(_unitOfWork.Set<Sender>()
                .ToList());
            SelectedSender = Senders.FirstOrDefault();
            Servers = new ObservableCollection<Server>(_unitOfWork.Set<Server>()
                .ToList());
            SelectedServer = Servers.FirstOrDefault();

            ScheduleTasks = new ObservableCollection<ScheduleTask>(_unitOfWork.Set<ScheduleTask>()
                .ToList());

            Messages = new ObservableCollection<Message>(_unitOfWork.Set<Message>()
                .ToList());

            SelectedMessage = Messages.FirstOrDefault();
        }

        #endregion
        
        #region Servers

        public ObservableCollection<Server> Servers
        {
            get =>
                _servers;
            set =>
                SetProperty(ref _servers,
                    value);
        }

        public Server SelectedServer
        {
            get =>
                _selectedServer;
            set =>
                SetProperty(ref _selectedServer,
                    value);
        }


        public ICommand AddServerCommand { get; }

        public ICommand RemoveServerCommand { get; }

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
            var password =  (string)server.Password.Clone();
            server = await _serverUserDialog.OpenEditDialogAsync(server);
            if (server is null)
                throw new NotImplementedException();
            if (server.Password != password)
                server.Password = await _encryptService.EncryptStringAsync(server.Password);
            server = await _unitOfWork.Set<Server>()
                .UpdateAsync(server);
            Servers.Add(server);
            await _unitOfWork.CommitAsync();
            SelectedServer = server;
        }

      

        #endregion

        #region Schedule

        public ObservableCollection<ScheduleTask> ScheduleTasks
        {
            get => _scheduleTasks;
            set => SetProperty(ref _scheduleTasks, value);
        }

        public DateTime SelectedDateTime
        {
            get => _selectedDateTime;
            set => SetProperty(ref _selectedDateTime, value);
        }

        public ICommand SendCommand { get; }

        private async void OnSend()
        {
            if (SelectedSender is null)
            {
                await _userDialogService.ShowErrorAsync("Не выбран отправитель", "Ошибка");
                return;
            }

            if (SelectedServer is null)
            {
                await _userDialogService.ShowErrorAsync("Не выбран сервер", "Ошибка");
                return;
            }

            if (SelectedMessage is null)
            {
                await _userDialogService.ShowErrorAsync("Не выбрано письмо.\nЕго можно выбрать на панели \"Редактор писем\"", "Ошибка");
                return;
            }
            
            if (SelectedRecipients is null || SelectedRecipients.Count == 0)
            {
                await _userDialogService.ShowErrorAsync("Не выбранs получательs", "Ошибка");
                return;
            }

            var result = await _emailSendService.SendAsync(
                SelectedServer,
                SelectedSender,
                SelectedRecipients,
                SelectedMessage);
            if (result != EmailSendResult.Success)
            {
                await _userDialogService.ShowErrorAsync("Ошибка отправки почты. Код: " + result,
                    "Ошибка отправки почты.");
                return;
            }

            await _userDialogService.ShowInformationAsync("Успешная отправка почты", "Успешная отправка почты");
        }

        #endregion

        #region Recipients
        public ObservableCollection<Recipient> SelectedRecipients
        {
            get => _selectedRecipients ??= new ObservableCollection<Recipient>(new[] {_selectedRecipient});
            set
            {
                SetProperty(ref _selectedRecipients, value);
                Debug.WriteLine($@"SelRecs: {DateTime.UtcNow}
{string.Join("\n", value)};
");
            }
        }

        public ObservableCollection<Recipient> Recipients
        {
            get =>
                _recipients;
            set =>
                SetProperty(ref _recipients,
                    value);
        }

        public Recipient SelectedRecipient
        {
            get =>
                _selectedRecipient;
            set =>
                SetProperty(ref _selectedRecipient,
                    value);
        }


        public ICommand AddRecipientCommand { get; }

        public ICommand RemoveRecipientCommand { get; }
        public ICommand EditRecipientCommand { get; }


        private async void OnAddRecipient()
        {
            var recipient = await _recipientUserDialog.OpenCreateDialogAsync();
            if (recipient is null)
                return;
            recipient = await _unitOfWork.Set<Recipient>()
                .AddAsync(recipient);
            Recipients.Add(recipient);
            await _unitOfWork.CommitAsync();
            SelectedRecipient = Recipients.LastOrDefault();
        }

        private async void OnRemoveRecipient()
        {
            if (SelectedRecipient is null)
                return;
            await _unitOfWork.Set<Recipient>()
                .RemoveAsync(SelectedRecipient);
            Recipients.Remove(SelectedRecipient);
            await _unitOfWork
                .CommitAsync();
            SelectedRecipient = Recipients.FirstOrDefault();
        }

        private async void OnEditRecipient()
        {
            if (SelectedRecipient is null)
                return;
            var recipient = SelectedRecipient;
            Recipients.Remove(SelectedRecipient);
            recipient = await _recipientUserDialog.OpenEditDialogAsync(recipient);
            recipient = await _unitOfWork.Set<Recipient>()
                .UpdateAsync(recipient);
            Recipients.Add(recipient);
            await _unitOfWork.CommitAsync();
            SelectedRecipient = recipient;
        }

        #endregion

        #region Senders

        public ObservableCollection<Sender> Senders
        {
            get =>
                _senders;
            set =>
                SetProperty(ref _senders,
                    value);
        }


        public Sender SelectedSender
        {
            get =>
                _selectedSender;
            set =>
                SetProperty(ref _selectedSender,
                    value);
        }


        public ICommand AddSenderCommand { get; }

        public ICommand RemoveSenderCommand { get; }
        public ICommand EditSenderCommand { get; }


        private async void OnAddSender()
        {
            var sender = await _senderUserDialog.OpenCreateDialogAsync();
            if (sender is null)
                return;
            sender = await _unitOfWork.Set<Sender>()
                .AddAsync(sender);
            Senders.Add(sender);
            await _unitOfWork.CommitAsync();
            SelectedSender = Senders.LastOrDefault();
        }

        private async void OnRemoveSender()
        {
            if (SelectedSender is null)
                return;
            await _unitOfWork.Set<Sender>()
                .RemoveAsync(SelectedSender);
            Senders.Remove(SelectedSender);
            await _unitOfWork
                .CommitAsync();
            SelectedSender = Senders.FirstOrDefault();
        }

        private async void OnEditSender()
        {
            if (SelectedSender is null)
                return;
            var sender = SelectedSender;
            Senders.Remove(SelectedSender);
            sender = await _senderUserDialog.OpenEditDialogAsync(sender);
            sender = await _unitOfWork.Set<Sender>()
                .UpdateAsync(sender);
            Senders.Add(sender);
            await _unitOfWork.CommitAsync();
            SelectedSender = sender;
        }

        #endregion

    }
}