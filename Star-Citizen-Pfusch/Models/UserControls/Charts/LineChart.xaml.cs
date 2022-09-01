using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace Star_Citizen_Pfusch.Models.UserControls.Charts
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        public LineChart()
        {
            InitializeComponent();
            Loaded += init;
        }
        private async void init(object sender, RoutedEventArgs e)
        {
            PointCollection points = new PointCollection();
            points.Add(new Point(0,ActualHeight));

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.URL + "/AccountData?History=true");
            request.Headers.Add("Token", Config.SessionToken);
            HttpResponseMessage response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();

            AccountDataItem item = JsonConvert.DeserializeObject<AccountDataItem>(res);
            double maxValue = item.PlaytimeHistory.Max();

            for (int i = 0; i < 25; i++)
            {
                Point point = new Point(ActualWidth / 25 * i, ActualHeight - (item.PlaytimeHistory[i] / maxValue * 60.0));
                points.Add(point);
            }
            points.Add(new Point(ActualWidth, ActualHeight));
            FilledPolygon.Points = points;
        }
    }
}
