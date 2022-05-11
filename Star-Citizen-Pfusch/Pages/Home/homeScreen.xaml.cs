﻿using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Pages.Ships;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Star_Citizen_Pfusch
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Page
    {
        public homeScreen()
        {
            ResourceDictionary dictionary = new ResourceDictionary();

            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dictionary.Source = new Uri("/languages/english.xaml", UriKind.Relative);
                    break;
                case "de-AT":
                    dictionary.Source = new Uri("/languages/german.xaml", UriKind.Relative);
                    break;
                case "de-DE":
                    dictionary.Source = new Uri("/languages/german.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dictionary);

            InitializeComponent();
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button button = (Button) sender;
            Indicator.Visibility = Visibility.Visible;
            
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

            Indicator.Visibility = Visibility.Hidden;
        }

        private void ShipButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDisplay.Content = new ShipList();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window.Title = "Settings";
            window.Content = new Pages.SettingsFolder.SettingsMenu();
            window.Owner = Application.Current.MainWindow;
            window.Width = 800;
            window.Height = 450;
            window.Show();
        }

        private void VehicleButton_Click(object sender, RoutedEventArgs e)
        {


        }

        private void fpsEquipmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void shipEquipmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDisplay.Content = new Pages.Home.Telemetry();
        }
    }
}
