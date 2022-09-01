using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

namespace Star_Citizen_Pfusch.Models.UserControls.Charts
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        private AccountDataItem item;
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
            PointCountBox.Text = item.PlaytimeHistory.Length.ToString();

            LoadChart();
        }
        private void LoadChart()
        {
            PointCollection points = new PointCollection();
            double Height = ActualHeight - 38;
            double Width = ActualWidth - 110;
            double maxValue = item.PlaytimeHistory.Max();

            points.Add(new Point(-1, Height));

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

            for (int i = 0; i < int.Parse(PointCountBox.Text); i++)
            {
                Point point = new Point(Width / (int.Parse(PointCountBox.Text) - 1) * i - 1, Height - (Height - 12) * (item.PlaytimeHistory[i] / maxValue));
                points.Add(point);
                
                Ellipse ellipse = new Ellipse() { Fill = new SolidColorBrush(Colors.LightBlue), Width = 6, Height = 6, Margin = new Thickness(point.X - 3, point.Y - 3, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ToolTip = formatePlayTime(item.PlaytimeHistory[i]), Style = (Style)FindResource("EllipseStyle") };
                Grid.SetRowSpan(ellipse, 5);
                Grid.SetColumn(ellipse, 1);
                Grid.Children.Add(ellipse);
            }
            points.Add(new Point(Width, Height));
            FilledPolygon.Points = points;
            PolygonLine.Points = points;
        }

        private void PointCountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (new Regex("^[0-9]+").IsMatch(e.Text) && int.Parse(((TextBox)sender).Text + e.Text) <= item.PlaytimeHistory.Length)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void PointCountBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.Equals("") && !((TextBox)sender).Text.Equals("1")) LoadChart();
        }
        private string formatePlayTime(int playtime)
        {
            int hour = playtime / 60;
            int minute = playtime - (hour * 60);
            return $"{hour}h {minute}m";
        }
    }
}
