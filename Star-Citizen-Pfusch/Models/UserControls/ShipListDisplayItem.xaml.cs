using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        public BitmapImage ImageSource { get; set; }
        public FleetItem FleetItem { get; set; }
        public string ShipName
        {
            get
            {
                return FleetItem.Name.Substring(FleetItem.Name.IndexOf(" ") + 1, FleetItem.Name.Length - FleetItem.Name.IndexOf(" ") - 1);
            }
        }
        public string Size
        {
            get
            {
                return "Size: " + FleetItem.ShipSize.Length + " x " + FleetItem.ShipSize.Width + " x " + FleetItem.ShipSize.Height;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            BackgroundBorder.Background = (LinearGradientBrush)this.Resources["HighlightBrush"];
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            BackgroundBorder.Background = (LinearGradientBrush)this.Resources["GradientBrush"];
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Height = this.ActualWidth / 1.6;
        }
    }
}
