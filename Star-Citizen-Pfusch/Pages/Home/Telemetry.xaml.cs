using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Cache;
using System.Net.Http;
using System.Reflection;
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

                NextPatchLabel.Content = formateDatetime(item.NextPatch - DateTime.Now);
                GameVersionLabel.Content = item.GameVersion;
                DailyShipDetails.Text = formateShipData(item.DailyShip);
                DailyShipDescription.Text = "Beschreibung:\n" + item.DailyShip.description;

                DailyShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + item.DailyShip.localName + ".jpg", UriKind.Relative));

                response = await client.GetAsync(Config.URL + "/AccountData");
                res = await response.Content.ReadAsStringAsync();

                AccountData data = JsonConvert.DeserializeObject<AccountData>(res);

                PlaytimeLabel.Content = data.Playtime;

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
                NextPatchLabel.Content = formateDatetime(item.NextPatch - DateTime.Now);
            });
        }

        private string formateShipData(ShipItem item)
        {
            return $"Name: {item.name}\n" +
                $"Größe: {item.length} x {item.width} x {item.height}\n" +
                $"Rolle: {item.role}\n" +
                $"Career: {item.career}";
        }
    }
}
