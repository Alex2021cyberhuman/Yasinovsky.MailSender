using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieSeller.Core.Models.Base;

namespace MovieSeller.Core.Models.Domain
{
    public class Booking<TKey> : IHasKey<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }

        public DateTime Booked { get; set; }

        [Range(0, int.MaxValue)]
        public int Count { get; set; }

        [ForeignKey(nameof(MovieSession<TKey>))]
        public TKey MovieSessionId { get; set; }

        public MovieSession<TKey> MovieSession { get; set; }
    }

    public class Booking : Booking<Guid> { }
}