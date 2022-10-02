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
    /// Interaction logic for ModuleDropdown.xaml
    /// </summary>
    public partial class ModuleDropdown : UserControl
    {
        public new SolidColorBrush Background { get; set; }
        public new SolidColorBrush BorderBrush { get; set; }
        public CornerRadius CornerRadius { get; set; }
        public string ModuleName { get; set; }
        public ModuleItem ModuleItem { get; set; }
        public ModuleDropdown()
        {
            InitializeComponent();
            this.DataContext = this;

            Loaded += init;
            
        }

        private void init(object sender, RoutedEventArgs e)
        {
            TypeImage.Source = new BitmapImage(new Uri($"/Graphics/{ModuleItem.Type}.png",UriKind.Relative));
        }
    }
}
