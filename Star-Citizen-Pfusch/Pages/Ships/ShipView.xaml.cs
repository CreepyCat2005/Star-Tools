using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Backend.Models;
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


namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ShipInformation.xaml
    /// </summary>
    public partial class ShipView : Page
    {
        private ObservableCollection<ListBoxItem> moduleItems = new ObservableCollection<ListBoxItem>();
        private FilterSettings filterSettings = new FilterSettings();
        private List<ModuleTypeEnum> checkedBoxes = new List<ModuleTypeEnum>();
        private ShipStatistics shipStatistics = new ShipStatistics();
        private ModuleStatistics moduleStatistics;

        public ShipView(string shipID)
        {
            init(shipID);

            checkedBoxes.AddRange(new ModuleTypeEnum[] { ModuleTypeEnum.Quantum_Drive, ModuleTypeEnum.Power_Plant, ModuleTypeEnum.Shield, ModuleTypeEnum.Cooler});
            filterSettings.CoolerBox.Click += ModuleBox_Checked;
            filterSettings.PowerPlantBox.Click += ModuleBox_Checked;
            filterSettings.QuantumDriveBox.Click += ModuleBox_Checked;
            filterSettings.ShieldBox.Click += ModuleBox_Checked;
            InitializeComponent();

            SettingsFrame.Navigate(shipStatistics);
        }

        private void ModuleBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox item = (CheckBox)sender;

            sortModules(item);
        }
        private void sortModules(CheckBox item)
        {

            switch (item.Content.ToString())
            {
                case "Quantum Drive":
                    if ((bool)item.IsChecked) checkedBoxes.Add(ModuleTypeEnum.Quantum_Drive);
                    else checkedBoxes.Remove(ModuleTypeEnum.Quantum_Drive);
                    break;
                case "Shield":
                    if ((bool)item.IsChecked) checkedBoxes.Add(ModuleTypeEnum.Shield);
                    else checkedBoxes.Remove(ModuleTypeEnum.Shield);
                    break;
                case "Cooler":
                    if ((bool)item.IsChecked) checkedBoxes.Add(ModuleTypeEnum.Cooler);
                    else checkedBoxes.Remove(ModuleTypeEnum.Cooler);
                    break;
                case "Power Plant":
                    if ((bool)item.IsChecked) checkedBoxes.Add(ModuleTypeEnum.Power_Plant);
                    else checkedBoxes.Remove(ModuleTypeEnum.Power_Plant);
                    break;
            }

            for (int i = 0; i < moduleItems.Count; i++)
            {
                DragAndDropItem dragAndDropItem = (DragAndDropItem)moduleItems[i].Content;
                if (!checkedBoxes.Contains(dragAndDropItem.type))
                {
                    moduleItems[i].Visibility = Visibility.Collapsed;
                }
                else
                {
                    moduleItems[i].Visibility = Visibility.Visible;
                }
            }
        }
        private string formate(string s)
        {
            for (int i = s.Length - 3; i > 0; i -= 3)
            {
                s = s.Insert(i, ".");
            }
            return s;
        }
        private async void init(string shipID)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Ship?ID={shipID}");
            string shipRes = await response.Content.ReadAsStringAsync();

            ShipItem item = JsonConvert.DeserializeObject<ShipItem>(shipRes);

            ShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + item.localName + ".jpg", UriKind.Relative));
            ShipStatus.Content = item.status;

            shipStatistics.ShipNameBox.Text += item.name;
            shipStatistics.ShipMassBox.Text += formate(item.hull.mass.ToString());
            shipStatistics.ShipSizeBox.Text += $"{item.data.size.x}m x {item.data.size.y}m x {item.data.size.z}m";
            shipStatistics.ShipRoleBox.Text += item.data.role;
            shipStatistics.ShipCareerBox.Text += item.data.career;
            shipStatistics.ShipDescriptionBox.Text += item.description;
            shipStatistics.ShipCargoBox.Text += formate(item.cargo.ToString()) + " scu";

            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();

            foreach (var shop in item.shops)
            {
                treeViewItems.Add(new TreeViewItem() { Foreground = new SolidColorBrush(Colors.White), FontSize = 25, Header = shop.name + " | " + shop.location + " | " + formate(shop.price.ToString()) + " UAC", Background = new SolidColorBrush(Colors.Transparent)});
            }

            shipStatistics.ShopTreeView.ItemsSource = treeViewItems;

            response = await client.GetAsync(Config.URL + "/Module/All");
            string moduleRes = await response.Content.ReadAsStringAsync();

            JArray jArray = JArray.Parse(moduleRes);

            List<string> validTypes = new List<string>(new string[] { "Shield", "PowerPlant", "QuantumDrive", "Cooler" });
            List<JToken> localNames = new List<JToken>(jArray.Children()["localName"]);
            int counter = 0;

            foreach (var element in JArray.Parse(JObject.Parse(shipRes)["modules"].ToString()))
            {
                if (element.Value<JToken>("itemTypes").Type != JTokenType.Null && element.Value<JToken>("itemTypes").HasValues && validTypes.Contains(element.Value<JToken>("itemTypes").Value<JToken>(0).Value<string>("type")))
                {
                    ModuleItem temp = JsonConvert.DeserializeObject<ModuleItem>(jArray[localNames.IndexOf(element.Value<string>("localName"))].ToString());

                    DragAndDropTarget dragAndDropTarget = new DragAndDropTarget() { Text = element.Value<JToken>("itemTypes").Value<JToken>(0).Value<string>("type") , type = (ModuleTypeEnum)temp.type, Size = "Size: " + temp.size.ToString() };
                    dragAndDropTarget.ContentFrame.Content = new DragAndDropItem() { QtNameText = temp.name, QtGradeText = "Grade: " + temp.grade, QtSizeText = "Size: " + temp.size.ToString(), type = (ModuleTypeEnum)temp.type };

                    ModuleGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid.SetColumn(dragAndDropTarget, counter);
                    ModuleGrid.Children.Add(dragAndDropTarget);

                    counter++;
                }
            }

            foreach (var module in jArray)
            {
                ModuleItem moduleItem = JsonConvert.DeserializeObject<ModuleItem>(module.ToString());
                if (moduleItem.size != getModuleSize((ModuleTypeEnum)moduleItem.type)) continue;
                DragAndDropItem dragAndDropItem = new DragAndDropItem()
                {
                    _id = moduleItem._id,
                    QtNameText = moduleItem.name,
                    QtGradeText = "Grade: " + moduleItem.grade,
                    QtSizeText = "Size: " + moduleItem.size.ToString(),
                    type = (ModuleTypeEnum)moduleItem.type,
                    Width = 130,
                    Height = 100
                };

                ListBoxItem boxItem = new ListBoxItem() { Content = dragAndDropItem };
                boxItem.MouseEnter += BoxItem_MouseEnter;
                boxItem.MouseLeave += BoxItem_MouseLeave;
                boxItem.MouseDoubleClick += BoxItem_MouseDoubleClick;

                moduleItems.Add(boxItem);
            }
            ModuleList.ItemsSource = moduleItems;
        }

        private void BoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragAndDropItem item = (DragAndDropItem)((ListBoxItem)sender).Content;
            moduleStatistics = new ModuleStatistics(item._id, (int)item.type);
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
            DragAndDropTarget[] targets = new DragAndDropTarget[10];
            ModuleGrid.Children.CopyTo(targets,0);

            foreach (var item in targets)
            {
                if (item.type == type) return int.Parse(item.Size.Replace("Size: ",""));
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

                sortModules(filterSettings.CoolerBox);
                sortModules(filterSettings.PowerPlantBox);
                sortModules(filterSettings.QuantumDriveBox);
                sortModules(filterSettings.ShieldBox);
            }
        }
    }
}
