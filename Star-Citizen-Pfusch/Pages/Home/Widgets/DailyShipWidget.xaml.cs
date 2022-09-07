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

namespace Star_Citizen_Pfusch.Pages.Home.Widgets
{
    /// <summary>
    /// Interaction logic for DailyShipWidget.xaml
    /// </summary>
    public partial class DailyShipWidget : UserControl
    {
        public string Details { get; set; }
        public string Description { get; set; }
        public BitmapSource Image { get; set; }
        public DailyShipWidget()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
