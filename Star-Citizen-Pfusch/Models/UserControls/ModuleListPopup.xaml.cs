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
                    gridView.Columns.Add(new GridViewColumn() { Header = "Efficieny", DisplayMemberBinding = new Binding("efficieny") { StringFormat = "{0:n}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Cooldown", DisplayMemberBinding = new Binding("cooldownTime") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Fuel requirement", DisplayMemberBinding = new Binding("quantumFuelRequirement") { StringFormat = "{0:n} /Mkm", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Range", DisplayMemberBinding = new Binding("range") { StringFormat = "{0:n0} km", ConverterCulture = CultureInfo.CurrentCulture } });
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
                case "WeaponGun":
                    gridView.Columns.Add(new GridViewColumn() { Header = "Alpha", DisplayMemberBinding = new Binding("Damage.alphaMax") { StringFormat = "{0:n0}", ConverterCulture = CultureInfo.CurrentCulture } });
                    gridView.Columns.Add(new GridViewColumn() { Header = "Ammo", DisplayMemberBinding = new Binding("Ammo.GetAmmo") { StringFormat = "{0}", ConverterCulture = CultureInfo.CurrentCulture } });
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

        private void ModuleListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            ModuleItem item = (ModuleItem)listView.SelectedItem;
            ModuleDropdown.ModuleName = item.Name;
            ModuleDropdown.ModuleItem = item;
        }

        //ModuleListView
        private void ColumnHeaderClick_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader header = (GridViewColumnHeader)e.OriginalSource;

            if (header.Name.Equals("Down"))
            {
                switch (header.Content)
                {
                    case "Name":
                        ModuleListView.ItemsSource = ModuleArray.OrderByDescending(o => o.Name);
                        break;
                    case "Size":
                        ModuleListView.ItemsSource = ModuleArray.OrderByDescending(o => o.Size);
                        break;
                    case "Grade":
                        ModuleListView.ItemsSource = ModuleArray.OrderByDescending(o => o.Grade);
                        break;
                    case "Cooling rate":
                        ModuleListView.ItemsSource = ModuleArray.Cast<CoolerItem>().OrderByDescending(o => o.CoolingRate);
                        break;
                    case "Speed":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderByDescending(o => o.driveSpeed);
                        break;
                    case "Efficieny":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderByDescending(o => o.efficieny);
                        break;
                    case "Cooldown":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderByDescending(o => o.cooldownTime);
                        break;
                    case "Fuel requirement":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderByDescending(o => o.quantumFuelRequirement);
                        break;
                    case "Range":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderByDescending(o => o.range);
                        break;
                    case "Shield health":
                        ModuleListView.ItemsSource = ModuleArray.Cast<ShieldItem>().OrderByDescending(o => o.MaxShieldHealth);
                        break;
                    case "Shield regen":
                        ModuleListView.ItemsSource = ModuleArray.Cast<ShieldItem>().OrderByDescending(o => o.MaxShieldRegen);
                        break;
                    case "Downed regen delay":
                        ModuleListView.ItemsSource = ModuleArray.Cast<ShieldItem>().OrderByDescending(o => o.DownedRegenDelay);
                        break;
                    case "Power":
                        ModuleListView.ItemsSource = ModuleArray.Cast<PowerPlantItem>().OrderByDescending(o => o.PowerBase);
                        break;
                    case "Time to draw request":
                        ModuleListView.ItemsSource = ModuleArray.Cast<PowerPlantItem>().OrderByDescending(o => o.TimeToReachDrawRequest);
                        break;
                    case "Alpha":
                        ModuleListView.ItemsSource = ModuleArray.Cast<WeaponItem>().OrderByDescending(o => o.Damage.alphaMax);
                        break;
                    case "Fire rate":
                        ModuleListView.ItemsSource = ModuleArray.Cast<WeaponItem>().OrderByDescending(o => o.Damage.fireRateMax);
                        break;
                    case "Ammo":
                        ModuleListView.ItemsSource = ModuleArray.Cast<WeaponItem>().OrderByDescending(o => o.Ammo.initialAmmoCount);
                        break;
                    default:
                        break;
                }
                header.Name = "Up";
            }
            else
            {
                switch (header.Content)
                {
                    case "Name":
                        ModuleListView.ItemsSource = ModuleArray.OrderBy(o => o.Name);
                        break;
                    case "Size":
                        ModuleListView.ItemsSource = ModuleArray.OrderBy(o => o.Size);
                        break;
                    case "Grade":
                        ModuleListView.ItemsSource = ModuleArray.OrderBy(o => o.Grade);
                        break;
                    case "Cooling rate":
                        ModuleListView.ItemsSource = ModuleArray.Cast<CoolerItem>().OrderBy(o => o.CoolingRate);
                        break;
                    case "Speed":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderBy(o => o.driveSpeed);
                        break;
                    case "Efficieny":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderBy(o => o.efficieny);
                        break;
                    case "Cooldown":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderBy(o => o.cooldownTime);
                        break;
                    case "Fuel requirement":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderBy(o => o.quantumFuelRequirement);
                        break;
                    case "Range":
                        ModuleListView.ItemsSource = ModuleArray.Cast<QuantumDriveItem>().OrderBy(o => o.range);
                        break;
                    case "Shield health":
                        ModuleListView.ItemsSource = ModuleArray.Cast<ShieldItem>().OrderBy(o => o.MaxShieldHealth);
                        break;
                    case "Shield regen":
                        ModuleListView.ItemsSource = ModuleArray.Cast<ShieldItem>().OrderBy(o => o.MaxShieldRegen);
                        break;
                    case "Downed regen delay":
                        ModuleListView.ItemsSource = ModuleArray.Cast<ShieldItem>().OrderBy(o => o.DownedRegenDelay);
                        break;
                    case "Power":
                        ModuleListView.ItemsSource = ModuleArray.Cast<PowerPlantItem>().OrderBy(o => o.PowerBase);
                        break;
                    case "Time to draw request":
                        ModuleListView.ItemsSource = ModuleArray.Cast<PowerPlantItem>().OrderBy(o => o.TimeToReachDrawRequest);
                        break;
                    case "Alpha":
                        ModuleListView.ItemsSource = ModuleArray.Cast<WeaponItem>().OrderBy(o => o.Damage.alphaMax);
                        break;
                    case "Fire rate":
                        ModuleListView.ItemsSource = ModuleArray.Cast<WeaponItem>().OrderBy(o => o.Damage.fireRateMax);
                        break;
                    case "Ammo":
                        ModuleListView.ItemsSource = ModuleArray.Cast<WeaponItem>().OrderBy(o => o.Ammo.initialAmmoCount);
                        break;
                    default:
                        break;
                }
                header.Name = "Down";
            }
        }
    }
}
