using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for ShipListDisplayItem.xaml
    /// </summary>
    public partial class ShipListDisplayItem : UserControl
    {
        public ShipListDisplayItem()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Border)sender).CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 35);
        }
        private void BorderSpecial_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Border)sender).CornerRadius = new CornerRadius(0, 0, (this.ActualHeight + this.ActualWidth) / 2 / 35, (this.ActualHeight + this.ActualWidth) / 2 / 35);
        }


        public Uri ImageSource { get; set; }
        public string ShipName { get; set; }
        public string ManufacturerName { get; set; }
        public List<string> ShipInfoItemsSource { get; set; }
    }
}
