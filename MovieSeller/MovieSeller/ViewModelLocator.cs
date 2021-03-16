using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MovieSeller.ViewModels;

namespace MovieSeller
{
    public class ViewModelLocator
    {
        public MovieSessionsViewModel MovieSessions => App.Host.Services.GetRequiredService<MovieSessionsViewModel>();

        public CreateNewMovieSessionViewModel CreateMovieSession => App.Host.Services.GetRequiredService<CreateNewMovieSessionViewModel>();
        public EditMovieViewModel EditMovie => App.Host.Services.GetRequiredService<EditMovieViewModel>();
    }
}
