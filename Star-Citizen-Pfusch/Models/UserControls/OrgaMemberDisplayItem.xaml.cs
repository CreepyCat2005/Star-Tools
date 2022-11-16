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
    /// Interaction logic for OrgaMemberDisplayItem.xaml
    /// </summary>
    public partial class OrgaMemberDisplayItem : UserControl
    {
        public OrgaMemberItem Item { get; set; }
        public string NickRank
        {
            get
            {
                return Item.Nickname + " / " + Item.Rank;
            }
        }

        public OrgaMemberDisplayItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
