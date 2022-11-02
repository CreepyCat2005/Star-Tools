using Newtonsoft.Json;
using Star_Citizen_Pfusch.Animations.Symbols;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Pages.Home.Widgets;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Star_Citizen_Pfusch.Pages.Home
{
    /// <summary>
    /// Interaction logic for Telemetry.xaml
    /// </summary>
    public partial class Telemetry : Page
    {
        private PublicDataItem item;
        private InformationWidget informationWidget;
        private ShipWatcher shipWatcher;
        private FundingWidget fundingWidget;
        private PlaytimeHistory playtime;

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
            for (int i = 0; i < MasterGrid.Children.Count; i++)
            {
                if (MasterGrid.Children[i].GetType() != typeof(GridSplitter))
                {
                    MasterGrid.Children.RemoveAt(i);
                    i--;
                }
            }

            //  adding information widget
            informationWidget = new InformationWidget()
            {
                PTUStatus = item.PTUStatus,
                GameVersion = item.gameVersion,
                ClientVersion = $"{version.Major}.{version.Minor}.{version.Build}"
            };
            if (data.PlaytimeHistory != null) informationWidget.Playtime = formatePlayTime(data.PlaytimeHistory.Sum() + (int)data.Playtime);
            else informationWidget.Playtime = formatePlayTime(0);
            Grid.SetColumn(informationWidget, 2);
            MasterGrid.Children.Add(informationWidget);

            //adding shipwatcher widget
            shipWatcher = new ShipWatcher();
            shipWatcher.Visibility = Visibility.Hidden;
            shipWatcher.OnUpdateStatus += ShipWatcher_OnUpdateStatus;
            Grid.SetRow(shipWatcher, 2);
            Grid.SetColumn(shipWatcher, 2);
            MasterGrid.Children.Add(shipWatcher);

            //adding funding widget
            fundingWidget = new FundingWidget();
            MasterGrid.Children.Add(fundingWidget);

            //adding DailyShip widget
            playtime = new PlaytimeHistory();
            Grid.SetRow(playtime, 2);
            MasterGrid.Children.Add(playtime);

        }

        private void ShipWatcher_OnUpdateStatus(object sender, Functions.StatusEventArgs e)
        {
            if (e.Status.Equals("Loaded"))
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
    }
}