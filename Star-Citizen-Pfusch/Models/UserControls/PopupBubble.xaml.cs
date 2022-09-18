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
    /// Interaction logic for PopupBubble.xaml
    /// </summary>
    public partial class PopupBubble : UserControl
    {

        public SolidColorBrush Color { get; set; }
        public string Text { get; set; }

        public PopupBubble()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Border)sender).CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 35);
        }
    }
}
