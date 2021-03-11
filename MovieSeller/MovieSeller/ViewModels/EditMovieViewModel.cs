using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MovieSeller.Core.Data;
using MovieSeller.Core.Extensions;
using MovieSeller.Core.Models.Domain;
using Yasinovsky.MailSender.Core.Models.Base;
using Yasinovsky.MailSender.Services.Wpf.Commands;
using static System.Guid;

namespace MovieSeller.ViewModels
{
    public class EditMovieViewModel : BaseViewModel
    {
        private readonly IMovieDataManager _movieDataManager;
        private Movie _selectedMovie;
        private ObservableCollection<Movie> _movies;

        public EditMovieViewModel(IMovieDataManager movieDataManager)
        {
            _movieDataManager = movieDataManager;

            CancelCommand = new CustomCommand(() => OnDialogClose(false));
            ConfirmMovieCommand = new WpfCustomCommand(
                ConfirmMovie, () => SelectedMovie is not null &&! string.IsNullOrWhiteSpace(SelectedMovie.Name) && SelectedMovie.Duration > TimeSpan.Zero);
            UpdateMovieCommand = new WpfCustomCommand(
                UpdateMovie, () => SelectedMovie is not null && !string.IsNullOrWhiteSpace(SelectedMovie.Name) && SelectedMovie.Duration > TimeSpan.Zero);
            AddMovieCommand = new WpfCustomCommand(
                AddMovie, () => SelectedMovie is not null && !string.IsNullOrWhiteSpace(SelectedMovie.Name) && SelectedMovie.Duration > TimeSpan.Zero);
            LoadCommand = new CustomCommand(Load);
        }

        

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set => SetProperty(ref _selectedMovie, value);
        }


        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set => SetProperty(ref _movies, value);
        }

        public ICommand UpdateMovieCommand { get; }

        public ICommand AddMovieCommand { get; }

        public ICommand ConfirmMovieCommand { get; }

        public ICommand LoadCommand { get; }

        public ICommand CancelCommand { get; }

        public event EventHandler<bool> DialogClose;

        protected virtual void OnDialogClose(bool e)
        {
            DialogClose?.Invoke(this, e);
        }

        private void Load()
        {
            if (SelectedMovie is not null || !Movies.Contains(SelectedMovie))
                SelectedMovie = Movies.FirstOrDefault(x => x.Name == SelectedMovie?.Name);
        }

        private async void ConfirmMovie()
        {
            await _movieDataManager.UpdateAsync(SelectedMovie);
            OnDialogClose(true);
        }

        private async void AddMovie()
        {
            var movie = (Movie)SelectedMovie.Clone();
            movie.Name = $"{SelectedMovie.Name}({Movies.Count(x => x.Name.StartsWith(SelectedMovie.Name)) + 1})";
            movie.Id = default;
            _selectedMovie = movie;
            SelectedMovie = movie;
            if (!Movies.Contains(movie))
                Movies.Add(movie);
            await _movieDataManager.UpdateAsync(movie);
        }

        private async void UpdateMovie()
        {
            if (!Movies.Contains(SelectedMovie))
                Movies.Add(SelectedMovie);
            await _movieDataManager.UpdateAsync(SelectedMovie);
        }
    }
}
