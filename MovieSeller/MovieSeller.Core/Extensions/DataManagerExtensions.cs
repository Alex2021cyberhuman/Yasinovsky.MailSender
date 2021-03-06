using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieSeller.Core.Data;
using MovieSeller.Core.Models.Base;

namespace MovieSeller.Core.Extensions
{
    public static class DataManagerExtensions
    {
        public static Task<IEnumerable<T>> GetAllAsync<T, TKey>(this IDataManager<T, TKey> source)
            where T : class, IHasKey<TKey> where TKey : IEquatable<TKey> =>
            source.GetWhereAsync(x => true);

        public static Task<T> FindById<T, TKey>(this IDataManager<T, TKey> source, TKey id) where T : class, IHasKey<TKey> where TKey : IEquatable<TKey> =>
            source.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }
}