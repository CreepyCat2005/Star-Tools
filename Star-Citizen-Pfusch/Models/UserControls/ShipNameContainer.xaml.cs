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
    /// Interaction logic for ShipNameContainer.xaml
    /// </summary>
    public partial class ShipNameContainer : UserControl
    {
        public string ShipName { get; set; }
        public string _id { get; set; }
        public ShipNameContainer()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
