﻿using System;
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
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Services.Wpf.Windows
{
    /// <summary>
    /// Interaction logic for ServerDialogWindow.xaml
    /// </summary>
    public partial class ServerDialogWindow : Window
    {
        public ServerDialogWindow(Server server)
        {
            InitializeComponent();
            DataContext = server;
        }
    }
}