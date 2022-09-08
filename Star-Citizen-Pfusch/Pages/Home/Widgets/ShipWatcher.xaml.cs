using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.UserControls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Text;
using Star_Citizen_Pfusch.Functions;

namespace Star_Citizen_Pfusch.Pages.Home.Widgets
{
    /// <summary>
    /// Interaction logic for ShipWatcher.xaml
    /// </summary>
    public partial class ShipWatcher : UserControl
    {
        public string ShipName { get; set; }
        public string ShipSale { get; set; }
        public string ShipPrice { get; set; }
        public ObservableCollection<ListBoxItem> listBoxItems { get; set; } = new ObservableCollection<ListBoxItem>();
        private bool IsBuyable = false;
        public delegate void StatusUpdateHandler(object sender, StatusEventArgs e);
        public event StatusUpdateHandler OnUpdateStatus;

        public ShipWatcher()
        {
            InitializeComponent();
            init();
            this.DataContext = this;
        }
        private void UpdateStatus(string status)
        {
            if (OnUpdateStatus == null) return;

            StatusEventArgs args = new StatusEventArgs(status);
            OnUpdateStatus(this,args);
        }
        private async void init()
        {
            HttpClient client = new HttpClient();
             
            HttpResponseMessage response = await client.GetAsync(Config.URL + "/Fleet?type=Vehicle_Spaceship&NamesOnly=true");
            string res = await response.Content.ReadAsStringAsync();

            JArray jArray = JArray.Parse(res);

            foreach (var item in jArray)
            {
                ListBoxItem listBoxItem = new ListBoxItem() { FontSize = 18, Content = new ShipNameContainer() { ShipName = item["name"].ToString(), LocalName = item["localName"].ToString() }, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent) };
                listBoxItem.MouseLeftButtonUp += AddShip_Click;
                ShipPopupList.Items.Add(listBoxItem);
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.URL + "/AccountData");
            request.Headers.Add("Token", Config.SessionToken);
            response = await client.SendAsync(request);
            res = await response.Content.ReadAsStringAsync();

            AccountDataItem accountItem = JsonConvert.DeserializeObject<AccountDataItem>(res);

            foreach (var item in accountItem.ShipsOnWatcher)
            {
                ListBoxItem listItem = new ListBoxItem() { FontSize = 20, Content = new ShipNameContainer() { ShipName = item.name, LocalName = item.localName }, BorderThickness = new Thickness(0) };
                listItem.MouseLeftButtonUp += LoadShipData_Click;
                listBoxItems.Add(listItem);
            }

            if(listBoxItems.Count > 0) LoadShip(((ShipNameContainer)listBoxItems[0].Content).LocalName);
            UpdateStatus("Loaded");
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsBuyable)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://robertsspaceindustries.com/pledge/Standalone-Ships/" + ((TextBlock)sender).Text.ToString(),
                    UseShellExecute = true
                });
            }
        }

        private void AddShip_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxItems.Select(o => ((ShipNameContainer)o.Content).LocalName).Contains(((ShipNameContainer)((ListBoxItem)sender).Content).LocalName)) return;
            ListBoxItem item = new ListBoxItem() { FontSize = 20, Content = new ShipNameContainer() { ShipName = ((ShipNameContainer)((ListBoxItem)sender).Content).ShipName , LocalName = ((ShipNameContainer)((ListBoxItem)sender).Content).LocalName}, BorderThickness = new Thickness(0) };
            item.MouseLeftButtonUp += LoadShipData_Click;
            listBoxItems.Add(item);
            ShipPopup.IsOpen = false;

            SendAccountDataUpdate();
        }
        private async void SendAccountDataUpdate()
        {
            List<ShipWatcherItem> shipWatcherItems = new List<ShipWatcherItem>();
            foreach (var item in listBoxItems)
            {
                shipWatcherItems.Add(new ShipWatcherItem() { name = ((ShipNameContainer)item.Content).ShipName, localName = ((ShipNameContainer)item.Content).LocalName});
            }
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(new AccountDataItem() { SessionToken = Config.SessionToken, ShipsOnWatcher = shipWatcherItems }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(Config.URL + "/AccountData",content);
            Debug.WriteLine(response.StatusCode);
        }
        private void LoadShipData_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)DeleteButton.IsChecked)
            {
                listBoxItems.Remove((ListBoxItem)sender);
                SendAccountDataUpdate();
                DeleteButton.IsChecked = false;
                return;
            }
            LoadShip(((ShipNameContainer)((ListBoxItem)sender).Content).LocalName);
        }
        private async void LoadShip(string localName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Ship?LocalName={localName}");
            string res = await response.Content.ReadAsStringAsync();

            ShipItem shipItem = JsonConvert.DeserializeObject<ShipItem>(res);

            ShipNameBox.Text = shipItem.name;
            ShipNameLayerBox.Text = shipItem.name;
            ShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + shipItem.localName + ".jpg", UriKind.Relative));
            if (shipItem.RealPrice != 0)
            {
                ShipSaleBox.Text = String.Format("{0:n} €", double.Parse(shipItem.RealPrice.ToString().Insert(shipItem.RealPrice.ToString().Length - 2, ",")));
                IsBuyable = true;
            }
            else
            {
                ShipSaleBox.Text = "Not in shop";
                IsBuyable = false;
            }
            if (shipItem.shops.Select(o => o.price).ToList().Count > 0)
            {
                ShipPriceBox.Text = String.Format("{0:n0} aUEC", shipItem.shops.Select(o => o.price).ToList()[0]);
            }
            else
            {
                ShipPriceBox.Text = "Not buyable";
            }
        }
    }
}
