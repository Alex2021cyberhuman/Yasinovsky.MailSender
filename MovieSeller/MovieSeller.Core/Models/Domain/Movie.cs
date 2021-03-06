using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MovieSeller.Core.Models.Base;

namespace MovieSeller.Core.Models.Domain
{
    public class Movie<TKey> : IHasKey<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public TimeSpan Duration { get; set; }

        public ICollection<MovieSession<TKey>> MovieSessions { get; set; }

        public override string ToString() => $"{{Id: {Id}, Name: {Name}}}";
    }

    public class Movie : Movie<Guid> { }
}