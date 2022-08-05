using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.Enums;
using Star_Citizen_Pfusch.Pages.Modules;
using Star_Citizen_Pfusch.Pages.SettingsFolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;


namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ShipInformation.xaml
    /// </summary>
    public partial class ShipView : Page
    {
        private List<ListBoxItem> moduleItems = new List<ListBoxItem>();
        private FilterSettings filterSettings = new FilterSettings();
        private List<ModuleTypeEnum> checkedBoxes = new List<ModuleTypeEnum>();
        private ShipStatistics shipStatistics = new ShipStatistics();
        private ModuleStatistics moduleStatistics;
        private ShipItem shipItem;

        public ShipView(string shipID)
        {
            init(shipID);

            checkedBoxes.AddRange(new ModuleTypeEnum[] { ModuleTypeEnum.Quantum_Drive, ModuleTypeEnum.Power_Plant, ModuleTypeEnum.Shield, ModuleTypeEnum.Cooler});
            filterSettings.CoolerBox.Checked += ModuleBox_Checked;
            filterSettings.PowerPlantBox.Checked += ModuleBox_Checked;
            filterSettings.QuantumDriveBox.Checked += ModuleBox_Checked;
            filterSettings.ShieldBox.Checked += ModuleBox_Checked;
            filterSettings.AllBox.Checked += ModuleBox_Checked;
            InitializeComponent();

            SettingsFrame.Navigate(shipStatistics);
        }
        private ModuleTypeEnum stringToModuleType(string s)
        {
            switch (s)
            {
                case "Quantum Drive":
                    return ModuleTypeEnum.Quantum_Drive;
                case "Cooler":
                    return ModuleTypeEnum.Cooler;
                case "Power Plant":
                    return ModuleTypeEnum.Power_Plant;
                case "Shield":
                    return ModuleTypeEnum.Shield;
                default:
                    return ModuleTypeEnum.Unknown;
            }
        }
        private ModuleTypeEnum checkBoxToModuleType(string s)
        {
            switch (s)
            {
                case "Speed":
                case "Efficiency":
                    return ModuleTypeEnum.Quantum_Drive;
                case "Cooling Rate":
                    return ModuleTypeEnum.Cooler;
                case "Power":
                    return ModuleTypeEnum.Power_Plant;
                case "Shield HP":
                case "Regeneration Rate":
                    return ModuleTypeEnum.Shield;
                default:
                    return ModuleTypeEnum.Unknown;
            }
        }
        private void filterModules(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;

            switch (box.Content.ToString())
            {
                case "Speed":

                    break;
                case "Efficiency":

                    break;
                case "Power":

                    break;
                case "Shield HP":

                    break;
                case "Regeneration Rate":

                    break;
                case "Cooling Rate":

                    break;
            }

        }
        private void ModuleBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox item = (CheckBox)sender;

            foreach (var element in filterSettings.AdvancedTreeItem.Items)
            {
                CheckBox box = (CheckBox)element;
                box.Checked += filterModules;
            }

            sortModules(item);
        }
        private void sortModules(CheckBox item)
        {
            for (int i = 0; i < moduleItems.Count; i++)
            {
                DragAndDropItem dragAndDropItem = (DragAndDropItem)moduleItems[i].Content;

                if (stringToModuleType(item.Content.ToString()) == dragAndDropItem.type || stringToModuleType(item.Content.ToString()) == ModuleTypeEnum.Unknown)
                {
                    moduleItems[i].Visibility = Visibility.Visible;
                }
                else
                {
                    moduleItems[i].Visibility = Visibility.Collapsed;
                }

            }
        }
        private async void init(string shipID)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Ship?ID={shipID}");
            string shipRes = await response.Content.ReadAsStringAsync();

            shipItem = JsonConvert.DeserializeObject<ShipItem>(shipRes);

            ShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + shipItem.localName + ".jpg", UriKind.Relative));
            ShipStatus.Content = shipItem.status;

            shipStatistics.ShipNameBox.Text += shipItem.name;
            shipStatistics.ShipMassBox.Text += String.Format("{0:n0} kg",shipItem.hull.mass);
            shipStatistics.ShipSizeBox.Text += $"{shipItem.data.size.y}m x {shipItem.data.size.x}m x {shipItem.data.size.z}m";
            shipStatistics.ShipRoleBox.Text += shipItem.data.role;
            shipStatistics.ShipCareerBox.Text += shipItem.data.career;
            shipStatistics.ShipDescriptionBox.Text += shipItem.description;
            shipStatistics.ShipCargoBox.Text += String.Format("{0:n0} scu",shipItem.cargo);

            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();

            foreach (var shop in shipItem.shops)
            {
                treeViewItems.Add(new TreeViewItem() { Foreground = new SolidColorBrush(Colors.White), FontSize = 25, Header = shop.name + " | " + shop.location + " | " + String.Format("{0:n0} aUEC",shop.price), Background = new SolidColorBrush(Colors.Transparent)});
            }

            shipStatistics.ShopTreeView.ItemsSource = treeViewItems;

            response = await client.GetAsync(Config.URL + "/Module/All");
            string moduleRes = await response.Content.ReadAsStringAsync();

            JArray jArray = JArray.Parse(moduleRes);

            List<string> validTypes = new List<string>(new string[] { "Shield", "PowerPlant", "QuantumDrive", "Cooler", "MissileLauncher" });
            List<JToken> localNames = new List<JToken>(jArray.Children()["localName"]);
            int counter = 0;
            ListBoxItem listBoxItem = new ListBoxItem()
            {
                Content = new Grid(),
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.White),
                Template = (ControlTemplate)Resources["ControlTemplate"],
                Height = 200
            };
            var element = JArray.Parse(JObject.Parse(shipRes)["modules"].ToString()).Where(o => o.Value<JToken>("itemTypes").Type != JTokenType.Null && o.Value<JToken>("itemTypes").HasValues && validTypes.Contains(o.Value<JToken>("itemTypes").Value<JToken>(0).Value<string>("type"))).ToList();

            List<int> distributedIntegers = DistributeInteger(element.Count, (int)Math.Ceiling(element.Count / 8.0)).ToList();
            int distributedIntegersCounter = 0;

            for (int i = 0; i < element.Count; i++)
            {
                Grid grid = (Grid)listBoxItem.Content;
                ModuleItem temp = JsonConvert.DeserializeObject<ModuleItem>(jArray[localNames.IndexOf(element[i].Value<string>("localName"))].ToString());

                DragAndDropTarget dragAndDropTarget = new DragAndDropTarget() { Text = element[i].Value<JToken>("itemTypes").Value<JToken>(0).Value<string>("type"), type = (ModuleTypeEnum)temp.type, Size = "Size: " + temp.size.ToString() };
                dragAndDropTarget.ContentFrame.Content = new DragAndDropItem() { QtNameText = temp.name, QtGradeText = "Grade: " + temp.grade, QtSizeText = "Size: " + temp.size.ToString(), type = (ModuleTypeEnum)temp.type };


                if (grid.Children.Count >= distributedIntegers[distributedIntegersCounter])
                {
                    ModuleTargetListBox.Items.Add(listBoxItem);
                    listBoxItem = new ListBoxItem() { Content = new Grid(), Background = new SolidColorBrush(Colors.Transparent), Foreground = new SolidColorBrush(Colors.White), Template = (ControlTemplate)Resources["ControlTemplate"], Height = 200 };
                    grid = (Grid)listBoxItem.Content;
                    counter = 0;
                    distributedIntegersCounter++;
                }
                if (grid.ColumnDefinitions.Count < distributedIntegers[distributedIntegersCounter]) grid.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(dragAndDropTarget, counter);
                grid.Children.Add(dragAndDropTarget);

                counter++;
            }
            ModuleTargetListBox.Items.Add(listBoxItem);

            foreach (var module in jArray)
            {
                ModuleItem moduleItem  = JsonConvert.DeserializeObject<ModuleItem>(module.ToString());
                object obj = null;

                switch ((ModuleTypeEnum)int.Parse(module["type"].ToString()))
                {
                    case ModuleTypeEnum.Quantum_Drive:
                        obj = JsonConvert.DeserializeObject<QuantumDriveItem>(module.ToString());
                        break;
                    case ModuleTypeEnum.Cooler:
                        obj = JsonConvert.DeserializeObject<CoolerItem>(module.ToString());
                        break;
                    case ModuleTypeEnum.Power_Plant:
                        obj = JsonConvert.DeserializeObject<PowerPlantItem>(module.ToString());
                        break;
                    case ModuleTypeEnum.Shield:
                        obj = JsonConvert.DeserializeObject<ShieldItem>(module.ToString());
                        break;
                    case ModuleTypeEnum.Mining:
                        obj = JsonConvert.DeserializeObject<MiningLaserItem>(module.ToString());
                        break;
                    case ModuleTypeEnum.Missile_Rack:
                        obj = JsonConvert.DeserializeObject<MissileRackItem>(module.ToString());
                        break;
                }

                if (moduleItem.size != getModuleSize((ModuleTypeEnum)moduleItem.type)) continue;
                double width = 130.0 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 1920.0);
                double heigth = 100.0 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 1080.0);
                DragAndDropItem dragAndDropItem = new DragAndDropItem()
                {
                    _id = moduleItem._id,
                    moduleItem = obj,
                    QtNameText = moduleItem.name,
                    QtGradeText = "Grade: " + moduleItem.grade,
                    QtSizeText = "Size: " + moduleItem.size.ToString(),
                    type = (ModuleTypeEnum)moduleItem.type,
                    Width = (int)width,
                    Height = (int)heigth
                };

                ListBoxItem boxItem = new ListBoxItem() { Content = dragAndDropItem };
                boxItem.MouseEnter += BoxItem_MouseEnter;
                boxItem.MouseLeave += BoxItem_MouseLeave;
                boxItem.MouseDoubleClick += BoxItem_MouseDoubleClick;

                moduleItems.Add(boxItem);
            }
            ModuleList.ItemsSource = moduleItems;
        }
        private static IEnumerable<int> DistributeInteger(int total, int divider)
        {
            if (divider == 0)
            {
                yield return 0;
            }
            else
            {
                int rest = total % divider;
                double result = total / (double)divider;

                for (int i = 0; i < divider; i++)
                {
                    if (rest-- > 0)
                        yield return (int)Math.Ceiling(result);
                    else
                        yield return (int)Math.Floor(result);
                }
            }
        }
        private void BoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragAndDropItem item = (DragAndDropItem)((ListBoxItem)sender).Content;
            moduleStatistics = new ModuleStatistics(item._id, (int)item.type, shipItem);
            SettingsFrame.Content = moduleStatistics;
        }

        private void BoxItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DragAndDropItem item = (DragAndDropItem)((ListBoxItem)sender).Content;
            item.BackgroundBorder.Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40));
        }

        private void BoxItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DragAndDropItem item = (DragAndDropItem)((ListBoxItem)sender).Content;
            item.BackgroundBorder.Background = new SolidColorBrush(Color.FromArgb(255,80,80,80));
        }

        private int getModuleSize(ModuleTypeEnum type)
        {
            List<DragAndDropTarget> targets = new List<DragAndDropTarget>();
            foreach (ListBoxItem item in ModuleTargetListBox.Items)
            {
                targets.AddRange(((Grid)item.Content).Children.OfType<DragAndDropTarget>());
            }

            foreach (var item in targets)
            {
                if (item != null && item.type == type) return int.Parse(item.Size.Replace("Size: ",""));
            }
            return 9354;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsFrame.Content == null || SettingsFrame.Content.GetType() != typeof(FilterSettings))
            {
                SettingsFrame.Navigate(filterSettings);
            }
            else
            {
                SettingsFrame.Content = null;
            }
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("AKfjsdkljghsdh");
        }

        private void ShipStatsButton_Click(object sender, RoutedEventArgs e)
        {

            if (SettingsFrame.Content == null || SettingsFrame.Content.GetType() != typeof(ShipStatistics))
            {
                SettingsFrame.Navigate(shipStatistics);
            }
            else
            {
                SettingsFrame.Content = null;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            string boxText = box.Text;
            if (box.Text.Equals("Search")) boxText = "";
            for (int i = 0; i < moduleItems.Count; i++)
            {
                DragAndDropItem dragAndDropItem = (DragAndDropItem)moduleItems[i].Content;
                if (!dragAndDropItem.QtNameText.Contains(boxText))
                {
                    moduleItems[i].Visibility = Visibility.Collapsed;
                }
                else
                {
                    moduleItems[i].Visibility = Visibility.Visible;
                }
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Text.Equals("Search"))
            {
                box.Text = "";
                box.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if(box.Text.Equals(""))
            {
                box.Text = "Search";
                box.Foreground = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));

                CheckBox[] checkBoxes = new CheckBox[] { filterSettings.AllBox, filterSettings.CoolerBox, filterSettings.PowerPlantBox, filterSettings.QuantumDriveBox, filterSettings.ShieldBox };

                for (int i = 0; i < checkBoxes.Length; i++)
                {
                    if ((bool)checkBoxes[i].IsChecked)
                    {
                        sortModules(checkBoxes[i]);
                        break;
                    }
                }
            }
        }
    }
}
