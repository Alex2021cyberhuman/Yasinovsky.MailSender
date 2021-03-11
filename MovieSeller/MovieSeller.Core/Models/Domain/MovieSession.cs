using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSeller.Core.Models.Base;
using Yasinovsky.MailSender.Core.Models.Base;

namespace MovieSeller.Core.Models.Domain
{
    public abstract class MovieSession<TKey> : BaseViewModel, IHasKey<TKey> where TKey: IEquatable<TKey>
    {
        private TKey _id;
        private DateTime _begin;
        private decimal _price;
        private int _maxCount;
        private TKey _movieId;
        private Movie _movie;
        private ICollection<Booking> _bookings;

        public virtual TKey Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public virtual DateTime Begin
        {
            get => _begin;
            set => SetProperty(ref _begin, value);
        }


        public virtual DateTime End => Begin.Add(Movie.Duration);

        [Range(0, double.MaxValue)]
        public virtual decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        [Range(0, int.MaxValue)]
        public virtual int MaxCount
        {
            get => _maxCount;
            set => SetProperty(ref _maxCount, value);
        }

        [ForeignKey(nameof(Movie))]
        public virtual TKey MovieId
        {
            get => _movieId;
            set => SetProperty(ref _movieId, value);
        }

        public virtual Movie Movie
        {
            get => _movie;
            set => SetProperty(ref _movie, value);
        }

        public int BookingCount => Bookings.Sum(x => x.Count);

        public virtual ICollection<Booking> Bookings
        {
            get => _bookings;
            set => SetProperty(ref _bookings, value);
        }
    }

    [Table("MovieSession")]
    public class MovieSession : MovieSession<Guid>
    {
    }
}
