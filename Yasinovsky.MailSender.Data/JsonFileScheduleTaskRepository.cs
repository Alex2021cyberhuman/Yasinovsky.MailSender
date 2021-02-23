using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Data
{
    public class JsonFileScheduleTaskRepository : JsonFileGenericRepository<ScheduleTask>
    {
        private readonly IUnitOfWork _unitOfWork;

        public JsonFileScheduleTaskRepository(JsonSerializerOptions options, FileInfo fileInfo, IUnitOfWork unitOfWork) : base(options, fileInfo)
        {
            _unitOfWork = unitOfWork;
        }

        public JsonFileScheduleTaskRepository(JsonSerializerOptions options, FileInfo fileInfo, IUnitOfWork unitOfWork, ICollection<ScheduleTask> initialBuffer) : base(options, fileInfo, initialBuffer)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<ScheduleTask> AddAsync(ScheduleTask item)
        {
            await Task.CompletedTask;
            lock (Buffer)
            {
                var itemFromList = Buffer.FirstOrDefault(x => x.Id == item.Id);
                if (itemFromList is not null)
                    throw new InvalidOperationException("Item already exists");
                AssignAdditionalInformation(item);
                Buffer.Add(item);
                IsChanged = true;
                return item;
            }
        }

        public override async Task<ScheduleTask> UpdateAsync(ScheduleTask item)
        {
            await Task.CompletedTask;
            lock (Buffer)
            {
                var itemFromList = Buffer.FirstOrDefault(x => x.Id == item.Id);
                if (itemFromList is null)
                    throw new InvalidOperationException("Item not found");
                Buffer.Remove(itemFromList);
                Buffer.Add(item);
                IsChanged = true;
                return item;
            }
        }

        protected override async Task InitializeAsync()
        {
            if (!_fileInfo.Exists || _fileInfo.Length < 2)
            {
                IsInitialized = true;
                IsChanged = true;
            }

            if (!IsInitialized)
            {
                IsInitialized = true;
                await using var reader = _fileInfo.OpenRead();
                Buffer = await JsonSerializer.DeserializeAsync<ICollection<ScheduleTask>>(reader, _options);
                foreach (var item in Buffer)
                {
                    AssignAdditionalInformation(item);
                }
                IsChanged = false;
            }

            
        }

        private void AssignAdditionalInformation(ScheduleTask item)
        {
            item.Id =
                (Buffer?.OrderBy(x => x.Id)
                        .LastOrDefault()?.Id ?? 0) + 1;
            if (item.Message is not null)
                item.MessageId = item.Message.Id;
            else
                item.Message = _unitOfWork.Set<Message>().First(x => x.Id == item.MessageId);
            if (item.Server is not null)
                item.ServerId = item.Server.Id;
            else
                item.Server = _unitOfWork.Set<Server>()
                    .First(x => x.Id == item.ServerId);

            if (item.Sender is not null)
                item.SenderId = item.Sender.Id;
            else
                item.Sender = _unitOfWork.Set<Sender>()
                .First(x => x.Id == item.SenderId);

            if (item.Recipients is not null)
                item.RecipientIds = item.Recipients.Select(x => x.Id)
                    .ToList();
            else
                item.Recipients = _unitOfWork.Set<Recipient>()
                    .Where(x => item.RecipientIds.Contains(x.Id))
                    .ToList();
        }
    }
}