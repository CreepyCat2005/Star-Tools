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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Animations.Symbols
{
    /// <summary>
    /// Interaction logic for LoadingSymbol.xaml
    /// </summary>
    public partial class LoadingSymbol : UserControl
    {
        public DoubleCollection HoleSize { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public LoadingSymbol()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
