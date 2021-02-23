using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Contracts.Data;
using Yasinovsky.MailSender.Core.Extensions;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Data
{
    // TODO: Make removing related data
    public class JsonFileGenericRepository<T> : IRepository<T> where T : IHasId
    {
        protected readonly JsonSerializerOptions _options;
        protected readonly FileInfo _fileInfo;
        
        private static ICollection<T> _buffer = new List<T>();

        public JsonFileGenericRepository(JsonSerializerOptions options, FileInfo fileInfo)
        {
            _options = options;
            _fileInfo = fileInfo;
        }

        public JsonFileGenericRepository(JsonSerializerOptions options, FileInfo fileInfo, ICollection<T> initialBuffer)
        {
            _options = options;
            _fileInfo = fileInfo;
            _buffer = initialBuffer;
            IsInitialized = true;
            IsChanged = true;
        }

        public ICollection<T> Buffer
        {
            get
            {
                if (!IsInitialized)
                    AsyncExtensions.RunSync(InitializeAsync);
                return _buffer;
            }
            set
            {
                _buffer = value;
                IsChanged = true;
            }
        }

        public bool IsChanged { get; set; }
        public bool IsInitialized { get; set; }

        public async Task CommitAsync()
        {
            await InitializeAsync();

            if (IsChanged)
            {
                _fileInfo.Delete();
                await using var writer = _fileInfo.OpenWrite();
                await JsonSerializer.SerializeAsync(writer, Buffer, _options);
                IsChanged = false;
                IsInitialized = true;
            }
        }

        protected virtual async Task InitializeAsync()
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
                Buffer = await JsonSerializer.DeserializeAsync<ICollection<T>>(reader, _options);
                IsChanged = false;
            }
        }


        public IEnumerator<T> GetEnumerator() => Buffer.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Type ElementType => Buffer.AsQueryable().ElementType;

        public Expression Expression => Buffer.AsQueryable().Expression;

        public IQueryProvider Provider => Buffer.AsQueryable().Provider;

        public virtual async Task<T> AddAsync(T item)
        {
            await Task.CompletedTask;
            lock (Buffer)
            {
                var itemFromList = Buffer.FirstOrDefault(x => x.Id == item.Id);
                if (itemFromList is not null)
                    throw new InvalidOperationException("Item already exists");
                item.Id = (Buffer?.OrderBy(x => x.Id).LastOrDefault()?.Id ?? 0) + 1;

                Buffer.Add(item);
                IsChanged = true;
                return item;
            }
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            await Task.CompletedTask;
            lock (Buffer)
            {
                var itemFromList = Buffer.FirstOrDefault(x =>x.Id == item.Id);
                if (itemFromList is null)
                    throw new InvalidOperationException("Item not found");
                Buffer.Remove(itemFromList);
                Buffer.Add(item);
                IsChanged = true;
                return item;
            }
        }

        public virtual async Task RemoveAsync(T item)
        {
            await Task.CompletedTask;
            lock (Buffer)
            {
                var itemFromList = Buffer.FirstOrDefault(x => x.Id == item.Id);
                if (itemFromList is null)
                    throw new InvalidOperationException("Item not found");
                Buffer.Remove(itemFromList);
                IsChanged = true;
            }
        }
    }
}