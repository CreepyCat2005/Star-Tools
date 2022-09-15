using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ModernShipList.xaml
    /// </summary>
    public partial class ModernShipList : Page
    {
        private ObservableCollection<ShipListDisplayItem> shipItems = new ObservableCollection<ShipListDisplayItem>();
        public ModernShipList(Frame frame, string type)
        {
            InitializeComponent();
            init(type);
            Unloaded += ModernShipList_Unloaded;
        }

        private void ModernShipList_Unloaded(object sender, RoutedEventArgs e)
        {
            shipItems.Clear();
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
                item.cargo = (int)item.cargo;

                List<string> infoList = new List<string>();
                infoList.Add("Role      " + item.data.role);
                infoList.Add("Career      " + item.data.career);
                infoList.Add("Cargo      " + item.cargo);
                infoList.Add("HP      " + item.hull.hp);
                infoList.Add("Size      " + item.data.size.x + " x " + item.data.size.y + " x " + item.data.size.z + " m");
                infoList.Add("Mass      " + item.hull.mass);
                infoList.Add("Hydrogen      " + item.fuelCapacity);
                infoList.Add("Quantum       " + item.qtFuelCapacity);

                ShipListDisplayItem displayItem = new ShipListDisplayItem()
                {
                    ShipName = item.name,
                    ManufacturerName = item.manufacturer.data.name,
                    ShipInfoItemsSource = infoList,
                    Margin = new Thickness(5),
                    Height = 300,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Graphics/ShipImages/Small/" + item.localName + ".jpg"))
                {
                    displayItem.ImageSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Graphics/ShipImages/Small/" + item.localName + ".jpg");
                    shipItems.Add(displayItem);
                }
            }
            //shipItems = shipItems.OrderBy(x => x.ShipName).ToList();

            ShipListBox.ItemsSource = shipItems;

            //disposing bullshit
            client.Dispose();
            response.Dispose();
        }
    }
}
