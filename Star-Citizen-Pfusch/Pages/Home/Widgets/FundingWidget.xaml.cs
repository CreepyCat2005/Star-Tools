using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
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

namespace Star_Citizen_Pfusch.Pages.Home.Widgets
{
    /// <summary>
    /// Interaction logic for FundingWidget.xaml
    /// </summary>
    public partial class FundingWidget : UserControl
    {
        public FundingWidget()
        {
            InitializeComponent();
            init("Week");
        }
        private async void init(string type)
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
                TextBox textBox = new TextBox() { HorizontalAlignment = HorizontalAlignment.Center, FontSize = 5, IsReadOnly = true, Text = String.Format("{0:n0}", item.gross / 100), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0) };
                textBox.SetResourceReference(ForegroundProperty, "TextColor");
                Rectangle rectangle = new Rectangle() { Margin = new Thickness(5, 0, 5, 0), Height = item.gross / maxValue * 60.0, Width = 20 };
                rectangle.SetResourceReference(Shape.FillProperty, "ChartColor");
                StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Bottom };
                stackPanel.Children.Add(rectangle);
                stackPanel.Children.Add(textBox);
                ChartDock.Children.Add(stackPanel);
            }
        }
        private void ChartComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!((ComboBoxItem)e.AddedItems[0]).Content.Equals(""))
            {
                init(((ComboBoxItem)e.AddedItems[0]).Content.ToString());
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
    }
}
