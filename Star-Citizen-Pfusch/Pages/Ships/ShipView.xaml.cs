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
using System.Threading.Tasks;
using System.Threading;

namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ShipInformation.xaml
    /// </summary>
    public partial class ShipView : Page
    {
        private ObservableCollection<ListBoxItem> moduleItems = new ObservableCollection<ListBoxItem>();
        private FilterSettings filterSettings = new FilterSettings();
        private ShipStatistics shipStatistics = new ShipStatistics();
        private ModuleStatistics moduleStatistics;
        private ShipItem shipItem;
        private JArray jArray;

        public ShipView(string shipID)
        {
            InitializeComponent();

            init(shipID);

            ModuleTargetListBox.Loaded += ModuleTargetListBox_Loaded;
            filterSettings.CoolerBox.Checked += ModuleBox_Checked;
            filterSettings.PowerPlantBox.Checked += ModuleBox_Checked;
            filterSettings.QuantumDriveBox.Checked += ModuleBox_Checked;
            filterSettings.ShieldBox.Checked += ModuleBox_Checked;
            filterSettings.MissileBox.Checked += ModuleBox_Checked;
            filterSettings.MissileRackBox.Checked += ModuleBox_Checked;
            filterSettings.AllBox.Checked += ModuleBox_Checked;

            SettingsFrame.Navigate(shipStatistics);
        }

        private void ModuleTargetListBox_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var module in jArray)
            {
                ModuleItem moduleItem = JsonConvert.DeserializeObject<ModuleItem>(module.ToString());
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
                    case ModuleTypeEnum.Missile:
                        obj = JsonConvert.DeserializeObject<MissileItem>(module.ToString());
                        break;
                }

                if (!getModuleSize((ModuleTypeEnum)moduleItem.type).Contains(moduleItem.size)) continue;
                double width = 130.0 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 1920.0);
                double heigth = 100.0 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 1080.0);
                DragAndDropItem dragAndDropItem = new DragAndDropItem()
                {
                    _id = moduleItem._id,
                    moduleItem = obj,
                    QtNameText = moduleItem.name,
                    QtGradeText = "Grade: " + moduleItem.grade,
                    QtSizeText = "Size: " + moduleItem.size,
                    Size = moduleItem.size,
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
                case "Missile":
                    return ModuleTypeEnum.Missile;
                case "Missile Rack":
                    return ModuleTypeEnum.Missile_Rack;
                default:
                    return ModuleTypeEnum.Unknown;
            }
        }
        private void filterModules(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            double width = 130.0 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 1920.0);
            double heigth = 100.0 * (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 1080.0);
            List<ListBoxItem> items = new List<ListBoxItem>();
            List<QuantumDriveItem> quantumDriveList;
            List<ShieldItem> shieldList;
            ListBoxItem listBox;

            switch (box.Content.ToString())
            {
                case "Speed":
                    quantumDriveList = moduleItems.OfType<ListBoxItem>().Select(o => o.Content).OfType<DragAndDropItem>().Select(o => o.moduleItem).OfType<QuantumDriveItem>().OrderByDescending(o => o.data.@params.driveSpeed).ToList();
                    foreach (var item in quantumDriveList)
                    {
                        DragAndDropItem dragAndDropItem = new DragAndDropItem()
                        {
                            _id = item._id,
                            moduleItem = item,
                            QtNameText = item.name,
                            QtGradeText = "Grade: " + item.grade,
                            QtSizeText = "Size: " + item.size,
                            Size = item.size,
                            type = (ModuleTypeEnum)item.type,
                            Width = (int)width,
                            Height = (int)heigth
                        };

                        listBox = new ListBoxItem();
                        listBox.MouseEnter += BoxItem_MouseEnter;
                        listBox.MouseLeave += BoxItem_MouseLeave;
                        listBox.MouseDoubleClick += BoxItem_MouseDoubleClick;
                        listBox.Content = dragAndDropItem;

                        items.Add(listBox);
                    }
                    for (int i = 0; i < moduleItems.Count; i++)
                    {
                        if (((DragAndDropItem)moduleItems[i].Content).moduleItem.GetType() == typeof(QuantumDriveItem))
                        {
                            moduleItems.Remove(moduleItems[i]);
                            i = 0;
                        }
                    }
                    break;
                case "Efficiency":
                    quantumDriveList = moduleItems.OfType<ListBoxItem>().Select(o => o.Content).OfType<DragAndDropItem>().Select(o => o.moduleItem).OfType<QuantumDriveItem>().OrderBy(o => o.data.quantumFuelRequirement).ToList();
                    foreach (var item in quantumDriveList)
                    {
                        DragAndDropItem dragAndDropItem = new DragAndDropItem()
                        {
                            _id = item._id,
                            moduleItem = item,
                            QtNameText = item.name,
                            QtGradeText = "Grade: " + item.grade,
                            QtSizeText = "Size: " + item.size,
                            Size = item.size,
                            type = (ModuleTypeEnum)item.type,
                            Width = (int)width,
                            Height = (int)heigth
                        };

                        listBox = new ListBoxItem();
                        listBox.MouseEnter += BoxItem_MouseEnter;
                        listBox.MouseLeave += BoxItem_MouseLeave;
                        listBox.MouseDoubleClick += BoxItem_MouseDoubleClick;
                        listBox.Content = dragAndDropItem;

                        items.Add(listBox);
                    }
                    for (int i = 0; i < moduleItems.Count; i++)
                    {
                        if (((DragAndDropItem)moduleItems[i].Content).moduleItem.GetType() == typeof(QuantumDriveItem))
                        {
                            moduleItems.Remove(moduleItems[i]);
                            i = 0;
                        }
                    }
                    break;
                case "Power":

                    break;
                case "Shield HP":
                    shieldList = moduleItems.OfType<ListBoxItem>().Select(o => o.Content).OfType<DragAndDropItem>().Select(o => o.moduleItem).OfType<ShieldItem>().OrderByDescending(o => o.data.maxShieldHealth).ToList();
                    foreach (var item in shieldList)
                    {
                        DragAndDropItem dragAndDropItem = new DragAndDropItem()
                        {
                            _id = item._id,
                            moduleItem = item,
                            QtNameText = item.name,
                            QtGradeText = "Grade: " + item.grade,
                            QtSizeText = "Size: " + item.size,
                            Size = item.size,
                            type = (ModuleTypeEnum)item.type,
                            Width = (int)width,
                            Height = (int)heigth
                        };

                        listBox = new ListBoxItem();
                        listBox.MouseEnter += BoxItem_MouseEnter;
                        listBox.MouseLeave += BoxItem_MouseLeave;
                        listBox.MouseDoubleClick += BoxItem_MouseDoubleClick;
                        listBox.Content = dragAndDropItem;

                        items.Add(listBox);
                    }
                    for (int i = 0; i < moduleItems.Count; i++)
                    {
                        if (((DragAndDropItem)moduleItems[i].Content).moduleItem.GetType() == typeof(ShieldItem))
                        {
                            moduleItems.Remove(moduleItems[i]);
                            i = 0;
                        }
                    }
                    break;
                case "Regeneration Rate":

                    break;
                case "Cooling Rate":

                    break;
            }
            for (int i = 0; i < items.Count; i++)
            {
                moduleItems.Add(items[i]);
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
        private void init(string shipID)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.GetAsync(Config.URL + $"/Ship?ID={shipID}").Result;
            string shipRes = response.Content.ReadAsStringAsync().Result;

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

            response = client.GetAsync(Config.URL + "/Module/All").Result;
            string moduleRes = response.Content.ReadAsStringAsync().Result;

            jArray = JArray.Parse(moduleRes);

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

                DragAndDropTarget dragAndDropTarget = new DragAndDropTarget() { Text = element[i].Value<JToken>("itemTypes").Value<JToken>(0).Value<string>("type"), type = (ModuleTypeEnum)temp.type, Size = temp.size };
                if (dragAndDropTarget.type == ModuleTypeEnum.Missile_Rack)
                {
                    MissileRackItem missileRack = JsonConvert.DeserializeObject<MissileRackItem>(jArray[localNames.IndexOf(element[i].Value<string>("localName"))].ToString());
                    MissileItem missile = JsonConvert.DeserializeObject<MissileItem>(jArray[localNames.IndexOf(element[i]["loadout"].Select(o => o.Value<JToken>("localName")).ToList()[0])].ToString());

                    DragAndDropFrame frame = new DragAndDropFrame() { Type = ModuleTypeEnum.Missile_Rack, ModuleName = temp.name, Size = missile.size };
                    frame.ContentFrame.Content = new DragAndDropItem() { type = ModuleTypeEnum.Missile, QtNameText = missile.name, Size = missile.size, QtSizeText = "Size: " + missile.size, QtGradeText = "Anzahl: " + missileRack.ports.Length };

                    dragAndDropTarget.ContentFrame.Content = frame;
                }
                else
                {
                    dragAndDropTarget.ContentFrame.Content = new DragAndDropItem() { QtNameText = temp.name, QtGradeText = "Grade: " + temp.grade, Size = temp.size, type = (ModuleTypeEnum)temp.type, QtSizeText = "Size: " + temp.size };
                }

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

        private List<int> getModuleSize(ModuleTypeEnum type)
        {
            List<DragAndDropTarget> targets = new List<DragAndDropTarget>();
            List<int> sizes = new List<int>();
            foreach (ListBoxItem item in ModuleTargetListBox.Items)
            {
                targets.AddRange(((Grid)item.Content).Children.OfType<DragAndDropTarget>());
            }

            foreach (var item in targets)
            {
                if (item != null && item.type == type && item.ContentFrame.Content.GetType() != typeof(DragAndDropFrame) || (item != null && item.type == type && item.ContentFrame.Content.GetType() == typeof(DragAndDropFrame) && ((DragAndDropFrame)item.ContentFrame.Content).Type == ModuleTypeEnum.Missile_Rack )) sizes.Add(item.Size);
                if (item != null && getSubType(item.type) == type && item.ContentFrame.Content.GetType() == typeof(DragAndDropFrame)) sizes.Add(((DragAndDropFrame)item.ContentFrame.Content).Size);
            }
            return sizes;
        }
        private ModuleTypeEnum getSubType(ModuleTypeEnum type)
        {
            switch (type)
            {
                case ModuleTypeEnum.Missile_Rack:
                    return ModuleTypeEnum.Missile;
                default:
                    return ModuleTypeEnum.Unknown;
            }
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
