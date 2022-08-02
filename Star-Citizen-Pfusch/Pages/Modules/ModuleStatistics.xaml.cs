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
using Star_Citizen_Pfusch.Models.Enums;
using Star_Citizen_Pfusch.Models;
using Newtonsoft.Json;
using System.Drawing;

namespace Star_Citizen_Pfusch.Pages.Modules
{
    /// <summary>
    /// Interaction logic for ModuleStatistics.xaml
    /// </summary>
    public partial class ModuleStatistics : Page
    {
        private List<ListBoxItem> listBoxItems = new List<ListBoxItem>();
        public ModuleStatistics(string _id, int type, ShipItem shipItem)
        {
            init(_id, type, shipItem);

            InitializeComponent();
        }

        private async void init(string _id, int type, ShipItem shipItem)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Module/Specific?_id={_id}&type={type}");
            string res = await response.Content.ReadAsStringAsync();

            switch ((ModuleTypeEnum)type)
            {
                case ModuleTypeEnum.Quantum_Drive:
                    QuantumDriveItem quantumDriveItem = JsonConvert.DeserializeObject<QuantumDriveItem>(res);
                    addStandartModules(quantumDriveItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Fuel-Requirement: {0:g}", quantumDriveItem.data.quantumFuelRequirement), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Speed: {0:n0}", quantumDriveItem.data.@params.driveSpeed), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Range: {0:n0}", (int)(shipItem.qtFuelCapacity / quantumDriveItem.data.quantumFuelRequirement) * 1000), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Disconnect-Range: {0:n0}", quantumDriveItem.data.disconnectRange), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Spooltime: {0:g}", quantumDriveItem.data.@params.spoolUpTime), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Cooldown: {0:g}", quantumDriveItem.data.@params.cooldownTime), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
                case ModuleTypeEnum.Power_Plant:
                    PowerPlantItem powerPlantItem = JsonConvert.DeserializeObject<PowerPlantItem>(res);
                    addStandartModules(powerPlantItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Power: {0:n0}", powerPlantItem.data.powerDraw), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Time to request: {0:g}", powerPlantItem.data.timeToReachDrawRequest), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Throttleable: " + powerPlantItem.data.isThrottleable, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Overclockable: " + powerPlantItem.data.isOverclockable, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Power to EM: {0:g}", powerPlantItem.data.powerToEM), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
                case ModuleTypeEnum.Shield:
                    ShieldItem shieldItem = JsonConvert.DeserializeObject<ShieldItem>(res);
                    addStandartModules(shieldItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Shield Health: {0:n0}", shieldItem.data.maxShieldHealth), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Downed Regeneration Delay: {0:g}", shieldItem.data.downedRegenDelay), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Damaged Regeneration Delay: {0:g}", shieldItem.data.damagedRegenDelay), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Maximum Regeneration: {0:n0}", shieldItem.data.maxShieldRegen), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
                case ModuleTypeEnum.Cooler:
                    CoolerItem coolerItem = JsonConvert.DeserializeObject<CoolerItem>(res);
                    addStandartModules(coolerItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Coolingrate: {0:n0}", coolerItem.data.coolingRate), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Supression IRF: {0:g}", coolerItem.data.suppressionIRFactor), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Supression Heat: {0:g}", coolerItem.data.suppressionHeatFactor), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
            }
            TreeView treeView = new TreeView()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0)
            };
            TreeViewItem treeViewItem = new TreeViewItem()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Header = "Shops",
                FontSize = 25,
                Background = new SolidColorBrush(Colors.Transparent)
            };
            treeView.Items.Add(treeViewItem);
            listBoxItems.Add(new ListBoxItem() { Content = treeView });

            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();

            foreach (var shop in JsonConvert.DeserializeObject<ModuleItem>(res).shops)
            {
                treeViewItems.Add(new TreeViewItem() { Foreground = new SolidColorBrush(Colors.White), FontSize = 25, Header = shop.name + " | " + shop.location + " | " + String.Format("{0:n0}",shop.price) + " aUEC", Background = new SolidColorBrush(Colors.Transparent) });
            }

            treeViewItem.ItemsSource = treeViewItems;
            ModuleInfoList.ItemsSource = listBoxItems;
        }
        private void addStandartModules(ModuleItem item)
        {
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Name: " + item.name, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Description: " + item.description, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25, TextWrapping = TextWrapping.Wrap } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Size: " + item.size, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Health: {0:n0}", item.health), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = String.Format("Mass: {0:n0} kg", item.mass), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Grade: " + item.grade, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
        }
    }
}
