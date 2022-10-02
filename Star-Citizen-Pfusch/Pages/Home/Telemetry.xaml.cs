using MongoDB.Bson;
using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Cache;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using System.Windows;
using Star_Citizen_Pfusch.Pages.Home.Widgets;
using Star_Citizen_Pfusch.Animations.Symbols;

namespace Star_Citizen_Pfusch.Pages.Home
{
    /// <summary>
    /// Interaction logic for Telemetry.xaml
    /// </summary>
    public partial class Telemetry : Page
    {
        private PublicDataItem item;
        private bool LoadedBefore = false;

        public Telemetry()
        {

            Loaded += OnLoad;
            Unloaded += Telemetry_Unloaded;

            InitializeComponent();
        }

        private void Telemetry_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Telemtry Unload");
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            if (LoadedBefore) return;
            Debug.WriteLine("Load");
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + "/PublicData");
            string res = await response.Content.ReadAsStringAsync();

            item = JsonConvert.DeserializeObject<PublicDataItem>(res);

            client.DefaultRequestHeaders.Add("token", Config.SessionToken);
            response = await client.GetAsync(Config.URL + "/AccountData?History=true");
            res = await response.Content.ReadAsStringAsync();

            AccountDataItem data = JsonConvert.DeserializeObject<AccountDataItem>(res);
            var version = Assembly.GetEntryAssembly().GetName().Version;

            //---------------------------------------adding widgets---------------------------------------------------

            //  adding information widget
            InformationWidget informationWidget = new InformationWidget()
            { 
                PTUStatus = item.PTUStatus,
                GameVersion = item.gameVersion,
                Playtime = formatePlayTime(data.PlaytimeHistory.Sum() + (int)data.Playtime),
                ClientVersion = $"{version.Major}.{version.Minor}.{version.Build}"
            };
            Grid.SetColumn(informationWidget, 2);
            MasterGrid.Children.Add(informationWidget);

            //adding shipwatcher widget
            ShipWatcher shipWatcher = new ShipWatcher();
            shipWatcher.Visibility = Visibility.Hidden;
            shipWatcher.OnUpdateStatus += ShipWatcher_OnUpdateStatus;
            Grid.SetRow(shipWatcher, 2);
            Grid.SetColumn(shipWatcher, 2);
            MasterGrid.Children.Add(shipWatcher);

            //adding funding widget
            FundingWidget fundingWidget = new FundingWidget();
            MasterGrid.Children.Add(fundingWidget);

            //adding DailyShip widget
            PlaytimeHistory playtime = new PlaytimeHistory();
            Grid.SetRow(playtime, 2);
            MasterGrid.Children.Add(playtime);

            LoadedBefore = true;
        }

        private void ShipWatcher_OnUpdateStatus(object sender, Functions.StatusEventArgs e)
        {
            if(e.Status.Equals("Loaded"))
            {
                ((ShipWatcher)sender).Visibility = Visibility.Visible;
                if (MasterGrid.Children.OfType<LoadingSymbol>().ToList().Count > 0)
                {
                    MasterGrid.Children.Remove(MasterGrid.Children.OfType<LoadingSymbol>().ToList()[0]);
                }
            }
        }

        private string formatePlayTime(int playtime)
        {
            int hour = playtime / 60;
            int minute = playtime - (hour * 60);
            return $"{hour}h {minute}m";
        }

        private string formatedata(ShipItem item)
        {
            return $"Name: {item.Name}\n" +
                $"Größe: {item.ShipSize.Length} x {item.ShipSize.Width} x {item.ShipSize.Height}\n" +
                $"Rolle: {item.Role}\n" +
                $"Career: {item.Career}\n" +
                $"Status: UNKNOWN";
        }
    }
}