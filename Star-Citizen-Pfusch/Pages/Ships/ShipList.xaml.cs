using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        public ShipList()
        {
            InitializeComponent();

            init();
        }

        private async void init()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.URL + "/Fleet");

                string input = await response.Content.ReadAsStringAsync();

                JObject json = JObject.Parse(input);

                for (int i = 0; i < (int)json["size"]; i++)
                {
                    this.ListBox.Items.Add(new ListBoxItem() { Content = json["items"][i]["name"], Height = 40, Foreground = Brushes.White });
                }
            }
        }
    }
}
