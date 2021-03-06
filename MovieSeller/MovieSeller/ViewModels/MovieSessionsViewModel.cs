using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MovieSeller.Core.Models.Domain;

namespace MovieSeller.ViewModels
{
    public class MovieSessionsViewModel
    {
        public ObservableCollection<MovieSession> MovieSessions { get; set; } = new ObservableCollection<MovieSession>(
            Enumerable.Range(1, 10)
                .Select(i => (i, Guid.NewGuid()))
                .Select(tuple => new MovieSession
                {
                    Id = tuple.Item2,
                    Begin = DateTime.Now, Bookings = new List<Booking<Guid>>(),
                    Movie = new Movie
                    {
                        Duration = TimeSpan.FromMinutes(tuple.i),
                        Id = tuple.Item2,
                        Name = "Movie" + tuple.Item2
                    }, MovieId = tuple.Item2,
                    Price = tuple.i
                }).ToList());

        public ICommand BuyBookingCommand { get; }

        public ICommand ChangeMovieCommand { get; }

        public ICommand RemoveSessionCommand { get; }
    }
}
