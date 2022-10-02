using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;

namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ShipStatistics.xaml
    /// </summary>
    public partial class ShipStatistics : Page
    {
        public string ShipName { get; set; }
        public string Size { get; set; }
        public string Mass { get; set; }
        public string Role { get; set; }
        public string Career { get; set; }
        public string Description { get; set; }
        public string Cargo { get; set; }

        public ShipStatistics()
        {

            InitializeComponent();
            this.DataContext = this;
        }
    }
}
