using MongoDB.Bson;
using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ShipInformation.xaml
    /// </summary>
    public partial class ShipView : Page
    {
        public ShipView(Frame frame, string shipID)
        {
            init(shipID);

            InitializeComponent();
        }

        private async void init(string shipID)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Ship?ID={shipID}");
                string res = await response.Content.ReadAsStringAsync();

                ShipItem item = JsonConvert.DeserializeObject<ShipItem>(res);

                ShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + item.localName + ".jpg", UriKind.Relative));
                ShipName.Text = item.name;
                ShipRole.Text = item.data.role;
                ShipCareer.Text = item.data.career;
                ShipDescription.Text = item.description;
                ShipSize.Text = $"{item.data.size.x} x {item.data.size.y} x {item.data.size.z}";
                ShipMass.Text = item.hull.mass.ToString();
                ShipCargo.Text = item.cargo.ToString();
                ShipHp.Text = item.health.ToString();
                //ShipShieldType.Text = item.shieldType;
                //ShipPrice.Text = item.price.ToString();
                ShipStatus.Content = item.status;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
