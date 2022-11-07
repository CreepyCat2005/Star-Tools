using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Pages.Home;
using Star_Citizen_Pfusch.Pages.Ships;
using Star_Citizen_Pfusch.Pages.Shops;
using Star_Citizen_Pfusch.Pages.Extras;
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
using Star_Citizen_Pfusch.Models.UserControls;
using System.Windows.Media.Effects;
using System.IO;
using Microsoft.Ink;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Media.Imaging;

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
        private NotesPage NotesPage = null;
        private ShopList ShopList = null;
        private PledgeList PledgeList = null;
        public homeScreen()
        {
            InitializeComponent();
            PlaytimeCounter.start(1000 * 60);
            InkCanvas.DefaultDrawingAttributes.Color = Colors.White;
        }
        private void MainMenuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Telemetry == null) Telemetry = new Telemetry();
            ContentDisplay.Navigate(Telemetry);
            InkCanvas.Visibility = Visibility.Collapsed;
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
                if (ModernShipList == null) ModernShipList = new ModernShipList(ContentDisplay, "Vehicle_Spaceship");
                ContentDisplay.Navigate(ModernShipList);
            }
            InkCanvas.Visibility = Visibility.Collapsed;
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
                if (ModernVehicleList == null) ModernVehicleList = new ModernShipList(ContentDisplay, "Vehicle_GroundVehicle");
                ContentDisplay.Navigate(ModernVehicleList);
            }
            InkCanvas.Visibility = Visibility.Collapsed;
        }

        private void PureShopDataItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ShopList == null) ShopList = new ShopList();
            ContentDisplay.Navigate(ShopList);
            InkCanvas.Visibility = Visibility.Collapsed;
        }
        private void Inventory_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PledgeList == null) PledgeList = new PledgeList();
            ContentDisplay.Navigate(PledgeList);
            InkCanvas.Visibility = Visibility.Collapsed;
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = new Popup()
            {
                AllowsTransparency = true,
                Placement = PlacementMode.Center,
                PlacementTarget = this,
                MinWidth = 500,
            };
            popup.Child = new ReportPopup(popup, ContentDisplay.Content.ToString())
            {
                Effect = new DropShadowEffect()
                {
                    ShadowDepth = 2,
                    Opacity = .6
                },
                Margin = new Thickness(10),
            };
            popup.IsOpen = true;
        }

        private void NotesListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NotesPage == null) NotesPage = new NotesPage();
            ContentDisplay.Navigate(NotesPage);
            InkCanvas.Visibility = Visibility.Collapsed;
        }

        private async void Orga_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://robertsspaceindustries.com/account/organization");
            request.Headers.Add("Cookie", LocalDataManager.GetRSICookieString());

            HttpResponseMessage response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();
            res = Regex.Replace(res, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string[] lineArray = res.Split("\n");

            for (int i = 0; i < lineArray.Length; i++)
            {
                if (lineArray[i].Contains("<div class=\"left-col\">"))
                {
                    OrgaStackPanel.Children.Add(new OrgaDisplayItem()
                    {
                        OrgaBackground = new SolidColorBrush(Colors.Transparent),
                        OrgaFontSize = 25,
                        CornerRadius = new CornerRadius(10),
                        Height = 50,
                        Image = new BitmapImage(new Uri("https://robertsspaceindustries.com" + lineArray[i + 3].Substring(lineArray[i + 3].IndexOf("><img src=\"") + 11, lineArray[i + 3].IndexOf("\" /></a>") - lineArray[i + 3].IndexOf("><img src=\"") - 11))),
                        OrgaName = lineArray[i + 9].Substring(lineArray[i + 9].IndexOf("class=\"value\">") + 14, lineArray[i + 9].IndexOf("</a>") - lineArray[i + 9].IndexOf("class=\"value\">") - 14),
                        Link = lineArray[i + 9].Substring(lineArray[i + 9].IndexOf("<a href=\"") + 9, lineArray[i + 9].IndexOf("\" class=\"value\">") - lineArray[i + 9].IndexOf("<a href=\"") - 9)
                    });
                }
            }
        }

        private void ContentDisplay_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Up) return;
            using (MemoryStream ms = new MemoryStream())
            {
                InkCanvas.Strokes.Save(ms);
                var myInkCollector = new InkCollector();
                var ink = new Ink();
                ink.Load(ms.ToArray());

                using (RecognizerContext context = new RecognizerContext())
                {
                    if (ink.Strokes.Count > 0)
                    {
                        context.Strokes = ink.Strokes;
                        RecognitionStatus status;

                        var result = context.Recognize(out status);

                        if (status == RecognitionStatus.NoError)
                            MessageBox.Show(result.TopString);
                        else
                            MessageBox.Show("Recognition failed");
                    }
                    else
                    {
                        MessageBox.Show("No stroke detected");
                    }
                }
            }
        }

        private void InkCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((InkCanvas)sender).Strokes.Clear();
        }
    }
}
