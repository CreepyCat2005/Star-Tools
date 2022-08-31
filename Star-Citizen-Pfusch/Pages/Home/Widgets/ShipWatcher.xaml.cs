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
        private bool IsBuyable = false;
        private List<ListBoxItem> listBoxItems;

        public ShipWatcher()
        {

            InitializeComponent();
            init();
            this.DataContext = this;

        }

        private async void init()
        {
            HttpClient client = new HttpClient();
             
            HttpResponseMessage response = await client.GetAsync(Config.URL + "/Fleet?type=Vehicle_Spaceship&NamesOnly=true");
            string res = await response.Content.ReadAsStringAsync();

            JArray jArray = JArray.Parse(res);

            foreach (var item in jArray)
            {
                ListBoxItem listBoxItem = new ListBoxItem() { FontSize = 18, Content = new ShipNameContainer() { ShipName = item["name"].ToString(), _id = item["_id"].ToString() }, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent) };
                listBoxItem.MouseLeftButtonUp += AddShip_Click;
                ShipPopupList.Items.Add(listBoxItem);
            }
            ShipList.ItemsSource = listBoxItems;
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
            ListBoxItem item = new ListBoxItem() { FontSize = 18, Content = new ShipNameContainer() { ShipName = ((ShipNameContainer)((ListBoxItem)sender).Content).ShipName , _id = ((ShipNameContainer)((ListBoxItem)sender).Content)._id}, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0) };
            item.MouseLeftButtonUp += LoadShipData_Click;
            listBoxItems.Add(item);
            ShipPopup.IsOpen = false;
        }

        private void RemoveShip_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void LoadShipData_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Ship?ID={((ShipNameContainer)((ListBoxItem)sender).Content)._id}");
            string res = await response.Content.ReadAsStringAsync();

            ShipItem shipItem = JsonConvert.DeserializeObject<ShipItem>(res);

            ShipNameBox.Text = shipItem.name;
            ShipNameLayerBox.Text = shipItem.name;
            ShipPriceBox.Text = String.Format("{0:n0} aUEC", shipItem.shops.Select(o => o.price).ToList()[0]);
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
        }
    }
}
