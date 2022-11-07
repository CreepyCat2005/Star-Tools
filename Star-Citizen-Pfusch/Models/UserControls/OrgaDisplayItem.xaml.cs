using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for OrgaDisplayItem.xaml
    /// </summary>
    public partial class OrgaDisplayItem : UserControl
    {
        public string OrgaName { get; set; }
        public BitmapImage Image { get; set; }
        public CornerRadius CornerRadius { get; set; }
        public double OrgaFontSize { get; set; }
        public SolidColorBrush OrgaBackground { get; set; }
        public string Link { get; set; }

        public OrgaDisplayItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Border_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}