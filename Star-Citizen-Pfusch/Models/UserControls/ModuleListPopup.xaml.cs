using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for ModuleListPopup.xaml
    /// </summary>
    public partial class ModuleListPopup : UserControl
    {
        public ModuleItem[] ModuleArray { get; set; }
        public ModuleDropdown ModuleDropdown { get; set; }
        public ModuleListPopup()
        {
            InitializeComponent();
            Loaded += init;
        }

        private void init(object sender, RoutedEventArgs e)
        {
            LoadGridViewConfig(ModuleArray[0].Type);

            ModuleListView.ItemsSource = ModuleArray;
        }
        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Border)sender).CornerRadius = new CornerRadius((this.ActualHeight + this.ActualWidth) / 2 / 20);
        }
        private void LoadGridViewConfig(string type)
        {
            GridView gridView = new GridView()
            {
                ColumnHeaderContainerStyle = (Style)Resources["GridViewHeaderStyle"],
            };
            AddStandardColumns(gridView);
            switch (type)
            {
                case "Cooler":
                    gridView.Columns.Add(new GridViewColumn() { Header = "Cooling rate", DisplayMemberBinding = new Binding("CoolingRate") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    break;
                case "QuantumDrive":
                    gridView.Columns.Add(new GridViewColumn() { Header = "Speed", DisplayMemberBinding = new Binding("driveSpeed") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Cooldown", DisplayMemberBinding = new Binding("cooldownTime") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Fuel requirement", DisplayMemberBinding = new Binding("quantumFuelRequirement") { StringFormat = "{0:n} /Mkm", ConverterCulture = CultureInfo.CurrentCulture } });
                    break;
                case "Shield":
                    gridView.Columns.Add(new GridViewColumn() { Header = "Shield health", DisplayMemberBinding = new Binding("MaxShieldHealth") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Shield regen", DisplayMemberBinding = new Binding("MaxShieldRegen") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Downed regen delay", DisplayMemberBinding = new Binding("DownedRegenDelay") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    break;
                case "PowerPlant":
                    gridView.Columns.Add(new GridViewColumn() { Header = "Power", DisplayMemberBinding = new Binding("PowerBase") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Time to draw request", DisplayMemberBinding = new Binding("TimeToReachDrawRequest") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    break;
                default:
                    break;
            }
            ModuleListView.View = gridView;
        }
        private void AddStandardColumns(GridView view)
        {
            view.Columns.Add(new GridViewColumn() { Header = "Name", DisplayMemberBinding = new Binding("Name") });
            view.Columns.Add(new GridViewColumn() { Header = "Size", DisplayMemberBinding = new Binding("Size") });
            view.Columns.Add(new GridViewColumn() { Header = "Grade", DisplayMemberBinding = new Binding("Grade") });
        }

        private void ModuleListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            ModuleItem item = (ModuleItem)listView.SelectedItem;
            ModuleDropdown.ModuleName = item.Name;
            ModuleDropdown.ModuleItem = item;
        }
    }
}
