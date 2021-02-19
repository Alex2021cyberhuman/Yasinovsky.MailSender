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
using Yasinovsky.MailSender.Services.Wpf.ViewModels;

namespace Yasinovsky.MailSender.Services.Wpf.Windows
{
    internal partial class MessageWindow : Window
    {
        public MessageWindow(MessageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.DialogReturn += ViewModelDialogReturn;
        }

        private void ViewModelDialogReturn(object sender, bool? e)
        {
            Close();
        }

        [Obsolete("Not work! Designer only ctor.")]
        public MessageWindow()
        {
            InitializeComponent();
        }
    }
}
