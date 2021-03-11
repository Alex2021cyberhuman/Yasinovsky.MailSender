using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieSeller.Core.Models.Base;
using Yasinovsky.MailSender.Core.Models.Base;

namespace MovieSeller.Core.Models.Domain
{
    public abstract class Booking<TKey> : BaseViewModel, IHasKey<TKey> where TKey : IEquatable<TKey>
    {
        private TKey _id;
        private DateTime _booked = DateTime.UtcNow;
        private int _count;
        private TKey _movieSessionId;
        private MovieSession _movieSession;

        public TKey Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public DateTime Booked
        {
            get => _booked;
            set => SetProperty(ref _booked, value);
        }

        [Range(0, int.MaxValue)]
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        [ForeignKey(nameof(MovieSession))]
        public TKey MovieSessionId
        {
            get => _movieSessionId;
            set => SetProperty(ref _movieSessionId, value);
        }

        public MovieSession MovieSession
        {
            get => _movieSession;
            set => SetProperty(ref _movieSession, value);
        }
    }

    [Table("Booking")]
    public class Booking : Booking<Guid> { }
}