using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for ShipWatcher.xaml
    /// </summary>
    public partial class ShipWatcher : UserControl
    {
        public string ShipName { get; set; }
        public string ShipSale { get; set; }
        public string ShipPrice { get; set; }
        public List<ListBoxItem> ItemsSource { get; set; }

        public ShipWatcher()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://robertsspaceindustries.com/pledge/Standalone-Ships/" + ((TextBlock)sender).Text.ToString(),
                UseShellExecute = true
            });
        }

        private void AddShip_Click(object sender, RoutedEventArgs e)
        {


            ItemsSource.Add(new ListBoxItem() { Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), Content = "HalloMama" });
        }

        private void RemoveShip_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
