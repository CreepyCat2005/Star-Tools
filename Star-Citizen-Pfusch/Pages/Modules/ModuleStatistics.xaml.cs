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
using System.Net.Http;
using System.Windows.Shapes;
using Star_Citizen_Pfusch.Models;
using Newtonsoft.Json;

namespace Star_Citizen_Pfusch.Pages.Modules
{
    /// <summary>
    /// Interaction logic for ModuleStatistics.xaml
    /// </summary>
    public partial class ModuleStatistics : Page
    {
        private List<ListBoxItem> listBoxItems = new List<ListBoxItem>();
        public ModuleStatistics(string _id, string type, ShipItem shipItem)
        {
            init(_id, type, shipItem);

            InitializeComponent();
        }

        private async void init(string _id, string type, ShipItem shipItem)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Module/Specific?_id={_id}&type={type}");
            string res = await response.Content.ReadAsStringAsync();

            switch (type)
            {
                case "QuantumDrive":
                    QuantumDriveItem quantumDriveItem = JsonConvert.DeserializeObject<QuantumDriveItem>(res);
                    addStandartModules(quantumDriveItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Fuel-Requirement: {0:g}", quantumDriveItem.quantumFuelRequirement), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Speed: {0:n0}", quantumDriveItem.driveSpeed), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Range: {0:n0}", (int)(shipItem.QuantumFuelCapacity / quantumDriveItem.quantumFuelRequirement) * 1000), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Disconnect-Range: {0:n0}", quantumDriveItem.disconnectRange), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    //listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Spooltime: {0:g}", quantumDriveItem.data.@params.spoolUpTime), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Cooldown: {0:g}", quantumDriveItem.cooldownTime), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
                case "PowerPlant":
                    PowerPlantItem powerPlantItem = JsonConvert.DeserializeObject<PowerPlantItem>(res);
                    addStandartModules(powerPlantItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Power: {0:n0}", powerPlantItem.PowerDraw), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Time to request: {0:g}", powerPlantItem.TimeToReachDrawRequest), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = "Throttleable: " + powerPlantItem.IsThrottleable, Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = "Overclockable: " + powerPlantItem.IsOverclockable, Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Power to EM: {0:g}", powerPlantItem.PowerToEM), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    break;
                case "Shield":
                    ShieldItem shieldItem = JsonConvert.DeserializeObject<ShieldItem>(res);
                    addStandartModules(shieldItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Shield Health: {0:n0}", shieldItem.MaxShieldHealth), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Downed Regeneration Delay: {0:g}", shieldItem.DownedRegenDelay), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Damaged Regeneration Delay: {0:g}", shieldItem.DamagedRegenDelay), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Maximum Regeneration: {0:n0}", shieldItem.MaxShieldRegen), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    break;
                case "Cooler":
                    CoolerItem coolerItem = JsonConvert.DeserializeObject<CoolerItem>(res);
                    addStandartModules(coolerItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Coolingrate: {0:n0}", coolerItem.CoolingRate), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Supression IRF: {0:g}", coolerItem.SuppressionIRFactor), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Supression Heat: {0:g}", coolerItem.SuppressionHeatFactor), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    break;
                case "Missile":
                    MissileItem MissileItem = JsonConvert.DeserializeObject<MissileItem>(res);
                    addStandartModules(MissileItem);
                    //listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = "Tracking Type: " + MissileItem.data.trackingSignalType, Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    //listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Min Range: {0:n0}", MissileItem.data.lockRangeMin), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    //listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Max Range: {0:n0}", MissileItem.data.lockRangeMax), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    //listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Speed: {0:n0}", MissileItem.data.linearSpeed), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Damage: {0:n0}", MissileItem.explosion.DamagePhysical), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    break;
                case "MissileRack": 
                    MissileRackItem MissileRackItem = JsonConvert.DeserializeObject<MissileRackItem>(res);
                    addStandartModules(MissileRackItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Port Count: {0:n0}", MissileRackItem.Ports.Length), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Port Size: {0:n0}", MissileRackItem.Ports[0].MaxSize), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });       
                    break;
            }
            TreeView treeView = new TreeView()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0)
            };
            TreeViewItem treeViewItem = new TreeViewItem()
            {
                Header = "Shops",
                FontSize = 25,
                Background = new SolidColorBrush(Colors.Transparent)
            };
            treeViewItem.SetResourceReference(ForegroundProperty, "TextColor");
            treeView.Items.Add(treeViewItem);
            listBoxItems.Add(new ListBoxItem() { Content = treeView });

            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();

            //TODO: Add shops to ModuleItem 
            //? Currently Deactivated the function!
            //foreach (var shop in JsonConvert.DeserializeObject<ModuleItem>(res).shops)
            //{
            //    TreeViewItem item = new TreeViewItem() { FontSize = 25, Header = shop.name + " | " + shop.location + " | " + String.Format("{0:n0}", shop.price) + " aUEC", Background = new SolidColorBrush(Colors.Transparent) };
            //    item.SetResourceReference(ForegroundProperty,"TextColor");
            //    treeViewItems.Add(item);
            //}

            treeViewItem.ItemsSource = treeViewItems;
            ModuleInfoList.ItemsSource = listBoxItems;
        }
        private void addStandartModules(ModuleItem item)
        {
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = "Name: " + item.Name, Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = "Description: " + item.Description, Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25, TextWrapping = TextWrapping.Wrap } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = "Size: " + item.Size, Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Health: {0:n0}", item.Health), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = String.Format("Mass: {0:n0} kg", item.Mass), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { Foreground = (SolidColorBrush)FindResource("TextColor"), IsReadOnly = true, Text = "Grade: " + item.Grade, Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
        }
    }
}
