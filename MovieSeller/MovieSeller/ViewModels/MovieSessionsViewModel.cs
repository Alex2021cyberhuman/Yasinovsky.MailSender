using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using MovieSeller.Core.Data;
using MovieSeller.Core.Models.Domain;
using MovieSeller.Core.Services;
using Yasinovsky.MailSender.Core.Models.Base;

namespace MovieSeller.ViewModels
{
    public class MovieSessionsViewModel
    {
        private readonly IMovieSessionDataManager _movieSessionDataManager;
        private readonly IServiceProvider _serviceProvider;

        public MovieSessionsViewModel(IMovieSessionDataManager movieSessionDataManager, IServiceProvider serviceProvider)
        {
            _movieSessionDataManager = movieSessionDataManager;
            _serviceProvider = serviceProvider;
            ChangeMovieCommand = new CustomCommand(ChangeMovie);
        }

        private async void ChangeMovie(object movieSessionObject)
        {
            var movieSession = (MovieSession) movieSessionObject;
            var navigationService = _serviceProvider.GetRequiredService<IDialogNavigationService>();
            var editMovieViewModel = _serviceProvider.GetRequiredService<EditMovieViewModel>();
            editMovieViewModel.Movie = (Movie) movieSession.Movie;
            await navigationService.ShowDialogAsync(nameof(EditMovieViewModel), editMovieViewModel);
            movieSession.Movie = editMovieViewModel.Movie;
            await _movieSessionDataManager.UpdateAsync(movieSession);
        }


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
