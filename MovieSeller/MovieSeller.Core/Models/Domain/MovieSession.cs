using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSeller.Core.Models.Base;

namespace MovieSeller.Core.Models.Domain
{
    public class MovieSession<TKey> : IHasKey<TKey> where TKey: IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }

        public virtual DateTime Begin { get; set; }


        public virtual DateTime End => Begin.Add(Movie.Duration);

        [Range(0, double.MaxValue)]
        public virtual decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public virtual int MaxCount { get; set; }

        [ForeignKey(nameof(Movie<TKey>))]
        public virtual TKey MovieId { get; set; }

        public virtual Movie<TKey> Movie { get; set; }

        public virtual ICollection<Booking<TKey>> Bookings { get; set; }
    }

    public class MovieSession : MovieSession<Guid>
    {
    }
}
