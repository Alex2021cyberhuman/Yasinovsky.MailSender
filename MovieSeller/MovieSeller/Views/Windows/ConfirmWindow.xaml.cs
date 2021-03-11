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
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        private ConfirmDeleteViewModel _viewModel;

        public ConfirmWindow()
        {
            InitializeComponent();
            DataContextChanged += ConfirmDeleteWindow_DataContextChanged;
        }

        private void ConfirmDeleteWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ConfirmDeleteViewModel viewModel)
            {
                if (_viewModel is not null)
                    _viewModel.DialogClose -= ViewModel_DialogClose;
                _viewModel = viewModel;
                _viewModel.DialogClose += ViewModel_DialogClose;
            }
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
