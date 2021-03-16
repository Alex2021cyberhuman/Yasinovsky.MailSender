using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using MovieSeller.Core.Data;
using MovieSeller.Core.Extensions;
using MovieSeller.Core.Models.Domain;
using MovieSeller.Core.Services;
using Yasinovsky.MailSender.Core.Models.Base;

namespace MovieSeller.ViewModels
{
    public class CreateNewMovieSessionViewModel : BaseViewModel
    {
        private MovieSession _movieSession;
        private IServiceProvider _serviceProvider;

        public CreateNewMovieSessionViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            MovieSession = new MovieSession();
            MovieSession.Begin = DateTime.Now.AddDays(1d);
            ChangeMovieCommand = new CustomCommand(ChangeMovie);
            CancelCommand = new CustomCommand(Cancel);
            SaveCommand = new CustomCommand(Save);
        }
        public MovieSession MovieSession
        {
            get => _movieSession;
            set => SetProperty(ref _movieSession, value);
        }

        public ICommand ChangeMovieCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand SaveCommand { get; }

        private async void Save()
        {
            if (MovieSession.Begin < DateTime.Now
                || MovieSession.Price < 0
                || MovieSession.MaxCount < 1
                || MovieSession.Movie is null)
                return;
                

            using var scope = _serviceProvider.CreateScope();
            var movieSessionsDataManager = scope.ServiceProvider.GetRequiredService<IMovieSessionDataManager>();
            try
            {
                await movieSessionsDataManager.UpdateAsync(MovieSession);
            }
            catch (Exception)
            {
                return;
            }
            
            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            await navigationService.GoBackAsync();
        }

        private async void Cancel()
        {
            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            await navigationService.GoBackAsync();
        }

        private async void ChangeMovie()
        {
            var movieSession = MovieSession;
            var navigationService = _serviceProvider.GetRequiredService<IDialogNavigationService>();
            var movieDataManager = _serviceProvider.GetRequiredService<IMovieDataManager>();
            var editMovieViewModel = _serviceProvider.GetRequiredService<EditMovieViewModel>();
            editMovieViewModel.SelectedMovie = movieSession.Movie;
            editMovieViewModel.Movies = new ObservableCollection<Movie>(
                await movieDataManager.GetAllAsync()
            );

            if (!await navigationService.ShowDialogAsync(nameof(EditMovieViewModel), editMovieViewModel)) return;
            movieSession.Movie = editMovieViewModel.SelectedMovie;
        }
    }
}
