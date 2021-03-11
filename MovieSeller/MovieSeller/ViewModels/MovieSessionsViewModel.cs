using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using MovieSeller.Core.Data;
using MovieSeller.Core.Extensions;
using MovieSeller.Core.Models.Domain;
using MovieSeller.Core.Services;
using Yasinovsky.MailSender.Core.Models.Base;
using Yasinovsky.MailSender.Services.Wpf.Commands;

namespace MovieSeller.ViewModels
{
    public class MovieSessionsViewModel : BaseViewModel
    {
        private readonly IMovieSessionDataManager _movieSessionDataManager;
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<MovieSession> _movieSessions;

        public MovieSessionsViewModel(IMovieSessionDataManager movieSessionDataManager, IServiceProvider serviceProvider)
        {
            _movieSessionDataManager = movieSessionDataManager;
            _serviceProvider = serviceProvider;
            ChangeMovieCommand = new CustomCommand(ChangeMovie);
            LoadCommand = new CustomCommand(Load);
            AddCommand = new CustomCommand(Add);
            BuyBookingCommand = new WpfCustomCommand(BuyBooking, o => 
                o is MovieSession m && m.BookingCount < m.MaxCount);
            RemoveSessionCommand = new CustomCommand(Remove);
            Debug.WriteLine(nameof(MovieSessionsViewModel));
            Debug.WriteLine(movieSessionDataManager.GetType().FullName);
            Debug.WriteLine(serviceProvider.GetType().FullName);
        }
        
        public ObservableCollection<MovieSession> MovieSessions
        {
            get => _movieSessions;
            set => SetProperty(ref _movieSessions, value);
        }

        public ICommand BuyBookingCommand { get; }

        public ICommand ChangeMovieCommand { get; }

        public ICommand RemoveSessionCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand AddCommand { get; }

        private async void BuyBooking(object obj)
        {
            var movieSession = (MovieSession)obj;
            var navigationService = _serviceProvider.GetRequiredService<IDialogNavigationService>();
            var buyBookingViewModel = _serviceProvider.GetRequiredService<BuyBookingViewModel>();
            buyBookingViewModel.MaxCount = movieSession.MaxCount - movieSession.BookingCount; 
            if (!await navigationService.ShowDialogAsync(nameof(BuyBookingViewModel), buyBookingViewModel)) return;
            movieSession.Bookings.Add(
                new Booking
                {
                    Booked = DateTime.UtcNow,
                    Count = buyBookingViewModel.Count
                });
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(MovieSessions)));
            await _movieSessionDataManager.UpdateAsync(movieSession);
        }

        private async void Remove(object obj)
        {
            var movieSession = (MovieSession)obj;
            var navigationService = _serviceProvider.GetRequiredService<IDialogNavigationService>();
            var viewModel = _serviceProvider.GetRequiredService<ConfirmDeleteViewModel>();
            viewModel.MovieSession = movieSession;
            if (!await navigationService.ShowDialogAsync(nameof(ConfirmDeleteViewModel), viewModel)) return;
            _movieSessions.Remove(movieSession);
            await _movieSessionDataManager.RemoveAsync(movieSession);
        }

        private void Add()
        {
            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            navigationService.NavigateToAsync(nameof(CreateNewMovieSessionViewModel));
        }

        private async void Load()
        {
            var movieSessionsEnumerable = await _movieSessionDataManager.GetAllAsync();
            MovieSessions = new ObservableCollection<MovieSession>(movieSessionsEnumerable);
        }

        private async void ChangeMovie(object obj)
        {
            var movieSession = (MovieSession) obj;
            var navigationService = _serviceProvider.GetRequiredService<IDialogNavigationService>();
            var movieDataManager = _serviceProvider.GetRequiredService<IMovieDataManager>();
            var editMovieViewModel = _serviceProvider.GetRequiredService<EditMovieViewModel>();
            editMovieViewModel.SelectedMovie = movieSession.Movie;
            editMovieViewModel.Movies = new ObservableCollection<Movie>(
                await movieDataManager.GetAllAsync()
            );

            if (!await navigationService.ShowDialogAsync(nameof(EditMovieViewModel), editMovieViewModel)) return;
            movieSession.Movie = editMovieViewModel.SelectedMovie;
            
            await _movieSessionDataManager.UpdateAsync(movieSession);
            Load();
        }

    }
}
