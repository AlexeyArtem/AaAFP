﻿using MahApps.Metro.Controls;
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

namespace AaAFP2
{
    /// <summary>
    /// Interaction logic for MaterialWindow.xaml
    /// </summary>
    public partial class PackingListWindow : MetroWindow
    {
        public PackingListWindow()
        {
            InitializeComponent();
            date.SelectedDate = DateTime.Today;
        }
    }
}
