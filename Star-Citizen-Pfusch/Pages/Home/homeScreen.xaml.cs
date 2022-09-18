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
        private ModernShipList ModernShipList = null;
        private ModernShipList ModernVehicleList = null;
        private ShipList VehicleList = null;
        private ShipList ShipList = null;
        private ShopList ShopList = null;
        private OrgaIntegration OrgaIntegration = null;
        private Point currentPoint = new Point();
        public homeScreen()
        {
            InitializeComponent();
            PlaytimeCounter.start(1000 * 60);
            ContentDisplay.Content = new Canvas() { Background = new SolidColorBrush(Colors.Transparent) };
        }
        private void MainMenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Telemetry == null) Telemetry = new Telemetry();
            ContentDisplay.Navigate(Telemetry);
        }

        private void ShipItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Config.ModernShipList)
            {
                if (ModernShipList == null) ModernShipList = new ModernShipList(ContentDisplay, "Vehicle_Spaceship");
                ContentDisplay.Navigate(ModernShipList);
            }
            else
            {
                if (ShipList == null) ShipList = new ShipList(ContentDisplay, "Vehicle_Spaceship");
                ContentDisplay.Navigate(ShipList);
            }
        }

        private void SettingsItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void Vehicle_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Config.ModernShipList)
            {
                if (ModernVehicleList == null) ModernVehicleList = new ModernShipList(ContentDisplay, "Vehicle_GroundVehicle");
                ContentDisplay.Navigate(ModernVehicleList);
            }
            else
            {
                if (VehicleList == null) VehicleList = new ShipList(ContentDisplay, "Vehicle_GroundVehicle");
                ContentDisplay.Navigate(VehicleList);
            }
        }

        private void PureShopDataItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ShopList == null) ShopList = new ShopList();
            ContentDisplay.Navigate(ShopList);
        }

        private void ContentFrame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                currentPoint = e.GetPosition(this);
            }
        }

        private void ContentFrame_MouseMove(object sender, MouseEventArgs e)
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
        private void ModuleListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        private void WeaponListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        private void UtilityListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void OrgaItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (OrgaIntegration == null) OrgaIntegration = new OrgaIntegration(ContentDisplay);
            ContentDisplay.Navigate(OrgaIntegration);
        }

    }
}
