using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Pages.Home;
using Star_Citizen_Pfusch.Pages.Ships;
using Star_Citizen_Pfusch.Pages.Shops;
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
        private Telemetry Telemetry = null;
        private ShipList ShipList = null;
        private ShipList VehicleList = null;
        private ShopList ShopList = null;
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
            PlaytimeCounter.start(1000 * 60);
        }
        private void MainMenuItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Telemetry == null)
            {
                Debug.WriteLine("New Telemetry");
                Telemetry = new Telemetry();
            }
            ContentDisplay.Navigate(Telemetry);
        }

        private void ShipItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ShipList == null) ShipList = new ShipList(ContentDisplay, "Vehicle_Spaceship");
            ContentDisplay.Content = null;
            ContentDisplay.Content = ShipList;
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

        private void Vehicle_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (VehicleList == null) VehicleList = new ShipList(ContentDisplay, "Vehicle_GroundVehicle");
            ContentDisplay.Content = null;
            ContentDisplay.Content = VehicleList;
        }

        private void ShopItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ShopList == null) ShopList = new ShopList();
            ContentDisplay.Content = null;
            ContentDisplay.Content = ShopList;
        }
    }
}
