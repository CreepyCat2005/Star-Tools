using System;
using System.Collections.Generic;
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
            ((Border)sender).CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
        }

        public ImageSource ImageSource { get; set; }
        public string ShipName { get; set; }
        public string ManufacturerName { get; set; }
        public string Cargo { get; set; }
        public string Weight { get; set; }
    }
}
