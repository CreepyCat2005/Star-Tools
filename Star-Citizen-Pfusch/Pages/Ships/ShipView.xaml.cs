using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        Popup popup = new Popup();
        public ShipView(Frame frame, int shipID)
        {
            init(shipID);

            InitializeComponent();
        }

        private async void init(int shipID)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Ship?ID={shipID}");
                string res = await response.Content.ReadAsStringAsync();

                ShipItem item = JsonConvert.DeserializeObject<ShipItem>(res);

                ShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + item.LocalName + ".jpg", UriKind.Relative));
                ShipName.Text = item.Name;
                ShipRole.Text = item.Role;
                ShipCareer.Text = item.Career;
                ShipDescription.Text = item.Description;
                ShipSize.Text = $"{item.Width} x {item.Length} x {item.Height}";
                ShipMass.Text = item.Mass.ToString();
                ShipCargo.Text = item.Cargo.ToString();
                ShipHp.Text = item.Hp.ToString();
                ShipShieldType.Text = item.ShieldType;
                ShipPrice.Text = item.Price.ToString();
                ShipStatus.Content = item.Status;

            }
        }
    }
}
