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
    /// Interaction logic for PledgeDisplayItem.xaml
    /// </summary>
    public partial class PledgeDisplayItem : UserControl
    {
        public BitmapImage ImageURI { get; set; }
        public string PledgeName { get; set; }
        public string PledgeCreated { get; set; }
        public string PledgeContains { get; set; }

        public PledgeDisplayItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
