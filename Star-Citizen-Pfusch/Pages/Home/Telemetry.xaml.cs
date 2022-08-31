using MongoDB.Bson;
using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Cache;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using System.Windows;

namespace Star_Citizen_Pfusch.Pages.Home
{
    /// <summary>
    /// Interaction logic for Telemetry.xaml
    /// </summary>
    public partial class Telemetry : Page
    {
        private PublicDataItem item;

        public Telemetry()
        {

            Loaded += OnLoad;
            Unloaded += Telemetry_Unloaded;

            InitializeComponent();
        }

        private void Telemetry_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Unload");
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Load");
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + "/PublicData");
            string res = await response.Content.ReadAsStringAsync();

            item = JsonConvert.DeserializeObject<PublicDataItem>(res);

            PTUStatusLabel.Content = item.PTUStatus;
            GameVersionLabel.Content = item.gameVersion;
            DailyShipDetails.Text = formatedata(item.dailyShip);
            DailyShipDescription.Text = "Beschreibung:\n" + item.dailyShip.description;

            DailyShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + item.dailyShip.localName + ".jpg", UriKind.Relative));

            client.DefaultRequestHeaders.Add("token", Config.SessionToken);
            response = await client.GetAsync(Config.URL + "/AccountData?History=true");
            res = await response.Content.ReadAsStringAsync();

            AccountDataItem data = JsonConvert.DeserializeObject<AccountDataItem>(res);
            var version = Assembly.GetEntryAssembly().GetName().Version;

            PlaytimeLabel.Content = formatePlayTime(data.PlaytimeHistory.Sum() + (int)data.Playtime);
            ClientVersionLabel.Content = $"{version.Major}.{version.Minor}.{version.Build}";



        }
        private async void loadFunding(string type)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/FundingData?type={type}");
            string res = await response.Content.ReadAsStringAsync();

            FundingItem fundingItem = JsonConvert.DeserializeObject<FundingItem>(res);
            PlayerTextBox.Text = String.Format("{0:n0}", fundingItem.fans);
            FundsTextBox.Text = String.Format("{0:n0}", fundingItem.funds / 100);
            ChartDock.Children.Clear();

            FundingDataItem[] chart = GetFundingDataItems(fundingItem, type);

            double maxValue = chart.Select(o => o.gross).Max();

            for (int i = 0; i < chart.Length; i++)
            {
                var item = chart[i];
                TextBox textBox = new TextBox() { HorizontalAlignment = HorizontalAlignment.Center ,FontSize = 5, IsReadOnly = true, Text = String.Format("{0:n0}",item.gross / 100), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0) };
                textBox.SetResourceReference(ForegroundProperty, "TextColor");
                Rectangle rectangle = new Rectangle() { Margin = new Thickness(5, 0, 5, 0), Height = item.gross / maxValue * 60.0, Width = 20 };
                rectangle.SetResourceReference(Shape.FillProperty, "ChartColor");
                StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Bottom };
                stackPanel.Children.Add(rectangle);
                stackPanel.Children.Add(textBox);
                ChartDock.Children.Add(stackPanel);
            }
        }
        private FundingDataItem[] GetFundingDataItems(FundingItem item, string type)
        {
            switch (type)
            {
                case "Day":
                    return item.day;
                case "Week":
                    return item.week;
                case "Month":
                    return item.month;
                default:
                    return null;
            }
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

        private void ChartComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((ComboBoxItem)e.AddedItems[0]).Content.Equals(""))
            {
                loadFunding(((ComboBoxItem)e.AddedItems[0]).Content.ToString());
            }
        }
    }
}