using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

namespace Star_Citizen_Pfusch.Pages.Integration
{
    /// <summary>
    /// Interaction logic for OrgaMenu.xaml
    /// </summary>
    public partial class OrgaMenu : Page
    {
        private ObservableCollection<ListBoxItem> listBoxItems = new ObservableCollection<ListBoxItem>();
        private string WebsiteLink, DiscordLink, YoutubeLink, TwitchLink;
        public OrgaMenu(ImageSource OrgaImage)
        {
            InitializeComponent();
            init(OrgaImage);
        }

        private async void init(ImageSource image)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.fleetyards.net/v1/fleets/starcomger/quick-stats");
            request.Headers.Add("Cookie", $"FLTYRD={LocalDataManager.GetFleetyardToken()}");
            HttpResponseMessage response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(res);
            string TotalMember = JObject.Parse(await client.GetAsync("https://api.fleetyards.net/v1/fleets/starcomger/member-quick-stats").Result.Content.ReadAsStringAsync())["total"].ToString();

            OrgaImage.Source = image;
            PeopleTextBox.Text = TotalMember;
            ShipTextBox.Text = json["total"].ToString();
            ValueTextBox.Text = String.Format("{0:n0}", json["metrics"]["totalMoney"]);

            //retrieving links
            request = new HttpRequestMessage(HttpMethod.Get, "https://api.fleetyards.net/v1/fleets/starcomger");
            request.Headers.Add("Cookie", $"FLTYRD={LocalDataManager.GetFleetyardToken()}");
            response = await client.SendAsync(request);
            res = await response.Content.ReadAsStringAsync();

            FleetYardOrgaItem orgaItem = JsonConvert.DeserializeObject<FleetYardOrgaItem>(res);
            WebsiteLink = "http://www." + orgaItem.homepage;
            DiscordLink = "http://www." + orgaItem.discord;
            YoutubeLink = "http://www." + orgaItem.youtube;
            TwitchLink = "http://www." + orgaItem.twitch;

            //filling list with ListBoxItems
            request = new HttpRequestMessage(HttpMethod.Get, "https://api.fleetyards.net/v1/fleets/starcomger/vehicles?perPage=all");
            request.Headers.Add("Cookie",$"FLTYRD={LocalDataManager.GetFleetyardToken()}");
            response = await client.SendAsync(request);
            res = await response.Content.ReadAsStringAsync();

            JArray jArray = JArray.Parse(res);

            for (int i = 0; i < jArray.Count; i++)
            {
                FleetyardShipItem shipItem = JsonConvert.DeserializeObject<FleetyardShipItem>(jArray[i].ToString());
                shipItem.ShipName = jArray[i]["model"]["name"].ToString(); ;
                shipItem.ImageURL = jArray[i]["model"]["storeImageSmall"].ToString();
                shipItem.localName = jArray[i]["model"]["scIdentifier"].ToString();

                if (listBoxItems.Where(o => ((FleetyardListBoxItem)o.Content).ShipName.Equals(shipItem.ShipName)).ToList().Count > 0)
                {
                    int index = listBoxItems.IndexOf(listBoxItems.Where(o => ((FleetyardListBoxItem)o.Content).ShipName.Equals(shipItem.ShipName)).ToList()[0]);
                    ((FleetyardListBoxItem)listBoxItems[index].Content).OwnerName += $" | {shipItem.Username}";
                    ((FleetyardListBoxItem)listBoxItems[index].Content).Count++;
                }
                else
                {
                    int GridColumn = i % 2;
                    ImageSource source;
                    if (!shipItem.localName.Equals("")) source = new BitmapImage(new Uri($"pack://application:,,,/Graphics/ShipImages/Small/{shipItem.localName}.jpg"));
                    else source = new BitmapImage(new Uri($"pack://application:,,,/Graphics/ShipImages/Small/{shipItem.ShipName}.jpg"));

                    ListViewItem listBoxItem = new ListViewItem() { Content = new FleetyardListBoxItem() { Count = 1, Source = source, ShipName = shipItem.ShipName, OwnerName = shipItem.Username }, Height = 150, VerticalContentAlignment = VerticalAlignment.Stretch, HorizontalContentAlignment = HorizontalAlignment.Stretch };


                    listBoxItems.Add(listBoxItem);
                }
            }
            ShipList.ItemsSource = listBoxItems;
        }

        private void Website_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (WebsiteLink.Equals("http://www.")) return;
            Process.Start(new ProcessStartInfo
            {
                FileName = WebsiteLink,
                UseShellExecute = true
            });
        }
        private void Discord_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DiscordLink.Equals("http://www.")) return;
            Process.Start(new ProcessStartInfo
            {
                FileName = DiscordLink,
                UseShellExecute = true
            });
        }
        private void Youtube_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (YoutubeLink.Equals("http://www.")) return;
            Process.Start(new ProcessStartInfo
            {
                FileName = YoutubeLink,
                UseShellExecute = true
            });
        }
        private void Twitch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TwitchLink.Equals("http://www.")) return;
            Process.Start(new ProcessStartInfo
            {
                FileName = TwitchLink,
                UseShellExecute = true
            });
        }
    }
}
