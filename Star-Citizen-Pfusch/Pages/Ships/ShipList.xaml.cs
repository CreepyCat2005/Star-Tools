using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ShipList.xaml
    /// </summary>
    public partial class ShipList : Page
    {
        private ShipView shipView;
        private Frame contentFrame;
        public ShipList(Frame frame)
        {
            contentFrame = frame;
            InitializeComponent();

            init();
        }

        private async void init()
        {
            ObservableCollection<ShipItem> shipItems = new ObservableCollection<ShipItem>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.URL + "/Fleet");

                string input = await response.Content.ReadAsStringAsync();

                JArray json = JArray.Parse(input);

                foreach (var entry in json)
                {
                    ShipItem item = JsonConvert.DeserializeObject<ShipItem>(entry.ToString());

                    shipItems.Add(item);
                }
            }
            ShipListView.ItemsSource = shipItems;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            ShipItem item = (ShipItem)listView.SelectedItem;
            shipView = new ShipView(contentFrame, item._id);
            contentFrame.Content = shipView;
        }
    }
}
