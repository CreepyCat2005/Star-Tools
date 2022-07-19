using MongoDB.Bson;
using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Cache;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Pages.Home
{
    /// <summary>
    /// Interaction logic for Telemetry.xaml
    /// </summary>
    public partial class Telemetry : Page
    {
        private System.Timers.Timer timer;
        private PublicDataItem item;

        public Telemetry()
        {

            Loaded += OnLoad;
            Unloaded += Telemetry_Unloaded;

            InitializeComponent();
        }

        private void Telemetry_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine("Unload");
            timer.Stop();
            timer.Dispose();
        }

        private async void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine("Load");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.URL + "/PublicData");
                string res = await response.Content.ReadAsStringAsync();

                item = JsonConvert.DeserializeObject<PublicDataItem>(res);

                NextPatchLabel.Content = formateDatetime(item.nextPatch - DateTime.Now);
                GameVersionLabel.Content = item.gameVersion;
                DailyShipDetails.Text = formatedata(item.dailyShip);
                DailyShipDescription.Text = "Beschreibung:\n" + item.dailyShip.description;

                DailyShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + item.dailyShip.localName + ".jpg", UriKind.Relative));

                client.DefaultRequestHeaders.Add("token",Config.SessionToken);
                response = await client.GetAsync(Config.URL + "/AccountData");
                res = await response.Content.ReadAsStringAsync();

                AccountData data = JsonConvert.DeserializeObject<AccountData>(res);

                PlaytimeLabel.Content = formatePlayTime(data.Playtime);

                timer = new System.Timers.Timer(1000);
                timer.Elapsed += elapsed;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
            ClientVersionLabel.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private string formateDatetime(TimeSpan time)
        {
            return $"{time.Days}:{time.Hours}:{time.Minutes}:{time.Seconds}";
        }
        private void elapsed(object source, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                NextPatchLabel.Content = formateDatetime(item.nextPatch - DateTime.Now);
            });
        }
        private string formatePlayTime(int playtime)
        {
            int hour = playtime / 60;
            int minute = playtime - (hour * 60);
            return $"{hour}h {minute}m";
        }

        private string formatedata(ShipItem item)
        {
            return $"Name: {item.name}\n" +
                $"Größe: {item.data.size.x} x {item.data.size.y} x {item.data.size.z}\n" +
                $"Rolle: {item.data.role}\n" +
                $"Career: {item.data.career}\n" +
                $"Status: {item.status}";
        }
    }
}
