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
    /// Interaction logic for BuyBookingWindow.xaml
    /// </summary>
    public partial class BuyBookingWindow : Window
    {
        private BuyBookingViewModel _viewModel;

        public BuyBookingWindow()
        {
            InitializeComponent();
            DataContextChanged += BuyBookingWindow_DataContextChanged;
        }

        private void BuyBookingWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is BuyBookingViewModel viewModel)
            {
                if (_viewModel is not null)
                    _viewModel.DialogClose -= ViewModel_DialogClose;
                _viewModel = viewModel;
                _viewModel.DialogClose += ViewModel_DialogClose;
            }
        }

        private void ViewModel_DialogClose(object sender, bool e)
        {
            Dispatcher.Invoke(() =>
            {
                DialogResult = e;
                Close();
            });
        }
    }
}
