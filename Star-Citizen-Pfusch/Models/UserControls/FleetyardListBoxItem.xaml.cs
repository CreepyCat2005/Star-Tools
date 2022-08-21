using System;
using System.Collections.Generic;
using System.IO;
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

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for FleetyardListBoxItem.xaml
    /// </summary>
    public partial class FleetyardListBoxItem : UserControl
    {
        public FleetyardListBoxItem()
        {
            InitializeComponent();

            this.DataContext = this;
        }
        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Border)sender).CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
        }

        public string OwnerName { get; set; }
        public string ShipName { get; set; }
        public int Count { get; set; }
        public ImageSource Source { get; set; }
    }
}
