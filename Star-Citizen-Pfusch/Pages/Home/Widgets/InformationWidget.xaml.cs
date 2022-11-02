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

namespace Star_Citizen_Pfusch.Pages.Home.Widgets
{
    /// <summary>
    /// Interaction logic for InformationWidget.xaml
    /// </summary>
    public partial class InformationWidget : UserControl
    {
        public InformationWidget()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string PTUStatus { get; set; }
        public string GameVersion { get; set; }
        public string Playtime { get; set; }
        public string ClientVersion { get; set; }

    }
}
