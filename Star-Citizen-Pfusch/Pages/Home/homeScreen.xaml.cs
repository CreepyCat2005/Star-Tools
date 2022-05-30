using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Pages.Home;
using Star_Citizen_Pfusch.Pages.Ships;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Star_Citizen_Pfusch
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Page
    {
        private Telemetry telemetry = null;
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

        private void MainMenuItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (telemetry == null)
            {
                Debug.WriteLine("New Telemetry");
                telemetry = new Telemetry();
            }
            ContentDisplay.Navigate(telemetry);
        }

        private void ShipItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContentDisplay.Content = null;
            ContentDisplay.Content = new ShipList(ContentDisplay);
        }

        private void SettingsItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Window window = new Window();
            window.Title = "Settings";
            window.Content = new Pages.SettingsFolder.SettingsMenu();
            window.Owner = Application.Current.MainWindow;
            window.Width = 800;
            window.Height = 450;
            window.Show();
        }
    }
}
