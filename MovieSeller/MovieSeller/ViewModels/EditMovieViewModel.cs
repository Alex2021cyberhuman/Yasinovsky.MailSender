using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MovieSeller.Core.Data;
using MovieSeller.Core.Models.Domain;
using Yasinovsky.MailSender.Core.Models.Base;
using Yasinovsky.MailSender.Services.Wpf.Commands;
using static System.Guid;

namespace MovieSeller.ViewModels
{
    public class EditMovieViewModel : BaseViewModel
    {
        private readonly IMovieDataManager _movieDataManager;
        private Movie _movie;

        public EditMovieViewModel()
        {
            Movies = new(Enumerable.Range(0, 10).Select(i => new Movie
            {
                Duration = TimeSpan.MaxValue,
                Id = NewGuid(),
                Name = "Movie" + i
            }).ToList());
            Movie = Movies[0];

            UpdateMovieCommand = new WpfCustomCommand(
                UpdateMovie, () => Movie is not null);
            AddMovieCommand = new WpfCustomCommand(
                AddMovie);
            ConfirmMovieCommand = new WpfCustomCommand(
                ConfirmMovie, () => Movie is not null);
        }

        private void ConfirmMovie()
        {
            OnDialogClose(true);
        }

        private void AddMovie()
        {
            var movie = new Movie();
            Movies.Add(movie);
        }

        private async void UpdateMovie()
        {
            await _movieDataManager.UpdateAsync(Movie);
        }

        public Movie Movie
        {
            get => _movie;
            set => SetProperty(ref _movie, value);
        }

        public event EventHandler<bool> DialogClose;

        public ObservableCollection<Movie> Movies { get; set; }

        public ICommand UpdateMovieCommand { get; }

        public ICommand AddMovieCommand { get; }

        public ICommand ConfirmMovieCommand { get; }

        protected virtual void OnDialogClose(bool e)
        {
            DialogClose?.Invoke(this, e);
        }
    }
}
