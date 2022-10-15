using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
using Star_Citizen_Pfusch.Functions;
using System.Diagnostics;
using MathNet.Numerics.Interpolation;

namespace Star_Citizen_Pfusch.Models.UserControls.Charts
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        private AccountDataItem item;
        private int offset;
        public LineChart()
        {
            InitializeComponent();
            Loaded += init;
        }
        private async void init(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.URL + "/AccountData?History=true");
            request.Headers.Add("Token", Config.SessionToken);
            HttpResponseMessage response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();

            item = JsonConvert.DeserializeObject<AccountDataItem>(res);
            if (item.PlaytimeHistory == null) return;
            if (NumberSelector.MinValue <= item.PlaytimeHistory.Length)
            {
                if (NumberSelector.Value == 0) NumberSelector.Value = item.PlaytimeHistory.Length;
                NumberSelector.MaxValue = item.PlaytimeHistory.Length;
            }
            else
            {
                return;
            }

            LoadChart();
        }
        private void LoadChart()
        {
            PointCollection points = new PointCollection();
            double Height = ActualHeight - 38;
            double Width = ActualWidth - 110;
            double maxValue = item.PlaytimeHistory.Max();
            if (maxValue == 0) maxValue = 1;

            RowNumber5.Text = formatePlayTime((int)maxValue);
            RowNumber4.Text = formatePlayTime((int)(maxValue * 0.8));
            RowNumber3.Text = formatePlayTime((int)(maxValue * 0.6));
            RowNumber2.Text = formatePlayTime((int)(maxValue * 0.4));
            RowNumber1.Text = formatePlayTime((int)(maxValue * 0.2));
            RowNumber0.Text = formatePlayTime(0);

            while (Grid.Children.OfType<Ellipse>().Count() != 0)
            {
                Grid.Children.Remove(Grid.Children.OfType<Ellipse>().First());
            }

            for (int i = 0; i < NumberSelector.Value; i++)
            {
                Point point = new Point(Width - (Width / (NumberSelector.Value - 1) * i - 1), Height - (Height - 12) * (item.PlaytimeHistory[i + offset] / maxValue));
                points.Add(point);

                var time = DateTime.Now - new TimeSpan(i, 0, 0, 0);

                Ellipse ellipse = new Ellipse() { Width = 8, Height = 8, Margin = new Thickness(points[i].X - 4, points[i].Y - 4, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ToolTip = formatePlayTime(item.PlaytimeHistory[i]) + "\n" + time.ToShortDateString(), Style = (Style)FindResource("EllipseStyle") };
                ellipse.SetResourceReference(Shape.FillProperty, "ChartPointColor");
                Grid.SetRowSpan(ellipse, 5);
                Grid.SetColumn(ellipse, 1);
                Grid.Children.Add(ellipse);
            }

            //interpolate
            var interpolation = CubicSpline.InterpolatePchip(points.Select(o => o.X), points.Select(o => o.Y));
            double max = points.Select(o => o.X).Max();
            int pointCount = points.Count * Config.ChartResolution;
            points.Clear();
            for (int i = 0; i <= pointCount; i++)
            {
                var cY = interpolation.Interpolate((max / pointCount) * i);
                points.Add(new Point(Width / pointCount * i, cY));
            }

            points.Add(new Point(Width, Height));
            points.Add(new Point(-1, Height));

            FilledPolygon.Points = points;
        }
        private string formatePlayTime(int playtime)
        {
            int hour = playtime / 60;
            int minute = playtime - (hour * 60);
            return $"{hour}h {minute}m";
        }

        private void NumberSelector_ValueChanged(object sender, ValueEventArgs e)
        {
            if (NumberSelector.Value + offset > item.PlaytimeHistory.Length) offset = item.PlaytimeHistory.Length - NumberSelector.Value;
            LoadChart();
        }

        private void Rectangle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && offset < item.PlaytimeHistory.Length - NumberSelector.Value)
            {
                offset++;
                LoadChart();
            }
            if (e.Delta < 0 && offset > 0)
            {
                offset--;
                LoadChart();
            }
        }
    }
}
