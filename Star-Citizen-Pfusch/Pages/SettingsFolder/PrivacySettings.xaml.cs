﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace Star_Citizen_Pfusch.Pages.SettingsFolder
{
    /// <summary>
    /// Interaction logic for PrivacySettings.xaml
    /// </summary>
    public partial class PrivacySettings : Page
    {
        public PrivacySettings()
        {
            InitializeComponent();
            this.DataContext = this;

            init();
        }

        private void init()
        {
            WebDisplay.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "Graphics\\Privacy.html");
        }
    }
}
