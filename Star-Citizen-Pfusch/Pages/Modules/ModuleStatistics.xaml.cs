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
using Star_Citizen_Backend.Models;
using Newtonsoft.Json;

namespace Star_Citizen_Pfusch.Pages.Modules
{
    /// <summary>
    /// Interaction logic for ModuleStatistics.xaml
    /// </summary>
    public partial class ModuleStatistics : Page
    {
        private List<ListBoxItem> listBoxItems = new List<ListBoxItem>();
        public ModuleStatistics(string _id, int type)
        {
            init(_id, type);

            InitializeComponent();
        }

        private async void init(string _id, int type)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Module/Specific?_id={_id}&type={type}");
            string res = await response.Content.ReadAsStringAsync();

            switch ((ModuleTypeEnum)type)
            {
                case ModuleTypeEnum.Quantum_Drive:
                    QuantumDriveItem quantumDriveItem = JsonConvert.DeserializeObject<QuantumDriveItem>(res);
                    addStandartModules(quantumDriveItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Fuel-Requirement: " + quantumDriveItem.data.quantumFuelRequirement, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Disconnect-Range: " + formate(quantumDriveItem.data.disconnectRange.ToString()), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Speed: " + formate(quantumDriveItem.data.@params.driveSpeed.ToString()), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Spooltime: " + quantumDriveItem.data.@params.spoolUpTime.ToString(), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Cooldown: " + quantumDriveItem.data.@params.cooldownTime.ToString(), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
                case ModuleTypeEnum.Power_Plant:
                    PowerPlantItem powerPlantItem = JsonConvert.DeserializeObject<PowerPlantItem>(res);
                    addStandartModules(powerPlantItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Power: " + powerPlantItem.data.powerDraw, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Time to request: " + powerPlantItem.data.timeToReachDrawRequest, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Throttleable: " + powerPlantItem.data.isThrottleable, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Overclockable: " + powerPlantItem.data.isOverclockable, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Power to EM: " + powerPlantItem.data.powerToEM, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
                case ModuleTypeEnum.Shield:
                    ShieldItem shieldItem = JsonConvert.DeserializeObject<ShieldItem>(res);
                    addStandartModules(shieldItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Shield Health: " + shieldItem.data.maxShieldHealth, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Downed Regeneration Delay: " + shieldItem.data.downedRegenDelay, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Damaged Regeneration Delay: " + shieldItem.data.damagedRegenDelay, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Maximum Regeneration: " + shieldItem.data.maxShieldRegen, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

                    break;
                case ModuleTypeEnum.Cooler:
                    CoolerItem coolerItem = JsonConvert.DeserializeObject<CoolerItem>(res);
                    addStandartModules(coolerItem);
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Coolingrate: " + coolerItem.data.coolingRate, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Supression IRF: " + coolerItem.data.suppressionIRFactor, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
                    listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Supression Heat: " + coolerItem.data.suppressionHeatFactor, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });

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
                treeViewItems.Add(new TreeViewItem() { Foreground = new SolidColorBrush(Colors.White), FontSize = 25, Header = shop.name + " | " + shop.location + " | " + formate(shop.price.ToString()) + " UAC", Background = new SolidColorBrush(Colors.Transparent) });
            }

            treeViewItem.ItemsSource = treeViewItems;
            ModuleInfoList.ItemsSource = listBoxItems;
        }
        private void addStandartModules(ModuleItem item)
        {
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Name: " + item.name, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Description: " + item.description, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25, TextWrapping = TextWrapping.Wrap } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Size: " + item.size, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Health: " + formate(item.health.ToString()), Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Mass: " + formate(item.mass.ToString()) + " kg", Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
            listBoxItems.Add(new ListBoxItem() { Content = new TextBox() { IsReadOnly = true, Text = "Grade: " + item.grade, Foreground = new SolidColorBrush(Colors.White), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(0), FontSize = 25 } });
        }

        private string formate(string s)
        {
            int index = s.Length;
            if (s.Contains(",")) index = s.LastIndexOf(","); 
            for (int i = index - 3; i > 0; i -= 3)
            {
                s = s.Insert(i,".");
            }
            return s;
        }
    }
}
