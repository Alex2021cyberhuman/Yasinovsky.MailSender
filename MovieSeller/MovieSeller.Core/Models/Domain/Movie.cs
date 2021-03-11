using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieSeller.Core.Models.Base;
using Yasinovsky.MailSender.Core.Models.Base;

namespace MovieSeller.Core.Models.Domain
{
    public abstract class Movie<TKey> : BaseViewModel, IHasKey<TKey> where TKey : IEquatable<TKey>
    {
        private TKey _id;
        private string _name;
        private TimeSpan _duration;
        private ICollection<MovieSession> _movieSessions;

        public TKey Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        [Required, StringLength(100)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public TimeSpan Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public ICollection<MovieSession> MovieSessions
        {
            get => _movieSessions;
            set => SetProperty(ref _movieSessions, value);
        }

        public override string ToString() => $"{{Id: {Id}, Name: {Name}}}";
    }

    [Table("Movie")]

    public class Movie : Movie<Guid>, ICloneable
    {
        public object Clone()
        {
            MovieSessions = null;
            return MemberwiseClone();
        }
    }
}