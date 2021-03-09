using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MovieSeller.ViewModels;

namespace MovieSeller.Views.Windows
{
    /// <summary>
    /// Interaction logic for EditMovieWindow.xaml
    /// </summary>
    public partial class EditMovieWindow
    {
        public EditMovieWindow()
        {
            InitializeComponent();
            var viewModel = (EditMovieViewModel) DataContext;
            viewModel.DialogClose += ViewModel_DialogClose;
        }

        private void ViewModel_DialogClose(object sender, bool e)
        {
            Dispatcher.Invoke(
                () =>
                {
                    DialogResult = e;
                    Close();
                });
        }
    }
}
