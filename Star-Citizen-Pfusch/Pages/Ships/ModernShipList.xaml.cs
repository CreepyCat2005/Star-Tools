using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ModernShipList.xaml
    /// </summary>
    public partial class ModernShipList : Page
    {
        private ObservableCollection<ShipListDisplayItem> shipItems = new ObservableCollection<ShipListDisplayItem>();
        private Frame frame;

        public ModernShipList(Frame frame, string type)
        {
            this.frame = frame;
            InitializeComponent();
            init(type);
        }
        private async void init(string type)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Config.URL + "/Fleet?type=" + type);

            string input = await response.Content.ReadAsStringAsync();

            JArray json = JArray.Parse(input);

            foreach (var entry in json)
            {
                FleetItem item = JsonConvert.DeserializeObject<FleetItem>(entry.ToString());
                item.Cargo = item.Cargo;

                List<string> infoList = new List<string>();
                infoList.Add("Role      " + item.Role);
                infoList.Add("Career      " + item.Career);
                infoList.Add("Cargo      " + item.Career);
                infoList.Add("HP      " + item.Health);
                infoList.Add("Size      " + item.ShipSize.Length + " x " + item.ShipSize.Width + " x " + item.ShipSize.Height + " m");
                infoList.Add("Mass      " + item.Mass);
                infoList.Add("Hydrogen      " + item.HydrogenFuelCapacity);
                infoList.Add("Quantum       " + item.QuantumFuelCapacity);

                BitmapImage image;

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Graphics\\ShipImages\\Small\\" + item.LocalName + ".jpg"))
                {
                    image = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Graphics\\ShipImages\\Small\\" + item.LocalName + ".jpg", UriKind.Absolute));
                }
                else
                {
                    image = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Graphics\\NoImage.png", UriKind.Absolute));
                }

                ShipListDisplayItem displayItem = new ShipListDisplayItem()
                {
                    FleetItem = item,
                    Margin = new Thickness(5),
                    Height = 300,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    ImageSource = image
                };
                displayItem.MouseLeftButtonUp += DisplayItem_MouseLeftButtonUp;

                shipItems.Add(displayItem);
            }
            //shipItems = shipItems.OrderBy(x => x.ShipName).ToList();
            ShipListBox.ItemsSource = shipItems;

            //disposing bullshit
            client.Dispose();
            response.Dispose();
        }

        private void DisplayItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (((ShipListDisplayItem)sender).FleetItem.Manufacturer != null)
            {
                frame.Navigate(new ModernShipView(((ShipListDisplayItem)sender).FleetItem._id));
                frame.RemoveBackEntry();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            for (int i = 0; i < shipItems.Count; i++)
            {
                shipItems[i].Visibility = Visibility.Visible;
                shipItems[i].Width = double.NaN;
                shipItems[i].Height = double.NaN;
            }
            TextBox textBox = (TextBox)sender;
            var items = shipItems.Where(o => !o.FleetItem.Name.ToLower().Contains(textBox.Text.ToLower())).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Visibility = Visibility.Collapsed;
                items[i].Width = 0;
                items[i].Height = 0;
            }
        }
    }
}
