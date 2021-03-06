using System;
using System.Windows;
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
