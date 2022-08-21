using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Pages.Home;
using Star_Citizen_Pfusch.Pages.Ships;
using Star_Citizen_Pfusch.Pages.Shops;
using Star_Citizen_Pfusch.Pages.Integration;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        private FleetyardIntegration FleetyardIntegration = null;
        private Point currentPoint = new Point();
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
            ContentDisplay.Content = new Canvas() { Background = new SolidColorBrush(Colors.Transparent) };
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
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = new Pages.SettingsFolder.SettingsMenu();
            window.Owner = Application.Current.MainWindow;
            window.Width = 800;
            window.MinWidth = 420;
            window.Height = 450;
            window.MinHeight = 300;
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

        private void ContentFrame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(this);
            }
        }

        private void ContentFrame_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && ContentDisplay.Content.GetType() == typeof(Canvas))
            {
                Line line = new Line();

                line.Stroke = new SolidColorBrush(Colors.White);
                line.StrokeThickness = 2;
                line.X1 = currentPoint.X - (Grid.ColumnDefinitions[0].Width.Value + Grid.ColumnDefinitions[1].Width.Value);
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X - (Grid.ColumnDefinitions[0].Width.Value + Grid.ColumnDefinitions[1].Width.Value);
                line.Y2 = e.GetPosition(this).Y;

                currentPoint = e.GetPosition(this);

                ((Canvas)ContentDisplay.Content).Children.Add(line);
            }
        }

        private void ContentDisplay_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ContentDisplay.Content.GetType() == typeof(Canvas))
            {
                ((Canvas)ContentDisplay.Content).Children.Clear();
            }
        }

        private void IntegrationItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (FleetyardIntegration == null) FleetyardIntegration = new FleetyardIntegration(ContentDisplay);
            ContentDisplay.Navigate(FleetyardIntegration);
        }
    }
}
