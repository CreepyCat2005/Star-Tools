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

namespace Star_Citizen_Pfusch.Pages.SettingsFolder
{
    /// <summary>
    /// Interaction logic for FilterSettings.xaml
    /// </summary>
    public partial class FilterSettings : Page
    {
        public FilterSettings()
        {
            InitializeComponent();
            AllBox.IsChecked = true;
        }

        private void Box_Checked(object sender, RoutedEventArgs e)
        {
            List<CheckBox> boxes = new List<CheckBox>(new CheckBox[] { PowerPlantBox, QuantumDriveBox, CoolerBox, ShieldBox, AllBox });
            boxes.Remove((CheckBox)sender);

            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].IsChecked = false;
            }

            setAdvancedFilters((CheckBox)sender);
        }
        private void FilterBox_Checked(object sender, RoutedEventArgs e)
        {
            List<CheckBox> boxes = new List<CheckBox>(AdvancedTreeItem.Items.OfType<CheckBox>());
            boxes.Remove((CheckBox)sender);

            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].IsChecked = false;
            }
        }
        private CheckBox createCheckbox(string s)
        {
            CheckBox check = new CheckBox() { Foreground = new SolidColorBrush(Colors.White), VerticalContentAlignment = VerticalAlignment.Center, Content = s };
            check.Checked += FilterBox_Checked;
            return check;
        }
        private void setAdvancedFilters(CheckBox box)
        {
            List<CheckBox> boxes = new List<CheckBox>();

            switch (box.Content)
            {
                case "Quantum Drive":
                    boxes.Add(createCheckbox("Speed"));
                    boxes.Add(createCheckbox("Efficiency"));
                    break;
                case "Power Plant":
                    boxes.Add(createCheckbox("Power"));
                    break;
                case "Shield":
                    boxes.Add(createCheckbox("Shield HP"));
                    boxes.Add(createCheckbox("Regeneration Rate"));
                    break;
                case "Cooler":
                    boxes.Add(createCheckbox("Cooling Rate"));
                    break;
                default:
                    break;
            }
            AdvancedTreeItem.ItemsSource = boxes;
        }
    }
}
