using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Backend.Models;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.Enums;
using Star_Citizen_Pfusch.Pages.SettingsFolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ShipInformation.xaml
    /// </summary>
    public partial class ShipView : Page
    {
        private ObservableCollection<ListBoxItem> moduleItems;
        private FilterSettings filterSettings = new FilterSettings();
        private List<ModuleTypeEnum> checkedBoxes = new List<ModuleTypeEnum>();
        private ShipStatistics shipStatistics = new ShipStatistics();

        public ShipView(Frame frame, string shipID)
        {
            init(shipID);

            filterSettings.CoolerBox.Click += ModuleBox_Checked;
            filterSettings.PowerPlantBox.Click += ModuleBox_Checked;
            filterSettings.QuantumDriveBox.Click += ModuleBox_Checked;
            filterSettings.ShieldBox.Click += ModuleBox_Checked;
            InitializeComponent();
        }

        private void ModuleBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox item = (CheckBox)sender;

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

        private async void init(string shipID)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(Config.URL + $"/Ship?ID={shipID}");
            string shipRes = await response.Content.ReadAsStringAsync();

            ShipItem item = JsonConvert.DeserializeObject<ShipItem>(shipRes);

            ShipImage.Source = new BitmapImage(new Uri(@"/Graphics/ShipImages/" + item.localName + ".jpg", UriKind.Relative));
            ShipStatus.Content = item.status;

            shipStatistics.ShipNameBox.Text += item.name;
            shipStatistics.ShipMassBox.Text += item.hull.mass;
            shipStatistics.ShipSizeBox.Text += $"{item.data.size.x}m x {item.data.size.y}m x {item.data.size.z}m";
            shipStatistics.ShipRoleBox.Text += item.data.role;
            shipStatistics.ShipCareerBox.Text += item.data.career;
            shipStatistics.ShipDescriptionBox.Text += item.description;
            shipStatistics.ShipCargoBox.Text += item.cargo;

            response = await client.GetAsync(Config.URL + "/Module");
            string moduleRes = await response.Content.ReadAsStringAsync();

            JArray jArray = JArray.Parse(moduleRes);

            moduleItems = new ObservableCollection<ListBoxItem>();

            foreach (var module in jArray)
            {
                ModuleItem moduleItem = JsonConvert.DeserializeObject<ModuleItem>(module.ToString());
                DragAndDropItem dragAndDropItem = new DragAndDropItem()
                {
                    QtNameText = moduleItem.name,
                    QtGradeText = moduleItem.grade,
                    QtSizeText = moduleItem.size.ToString(),
                    type = (ModuleTypeEnum)moduleItem.type,
                    Width = 130,
                    Height = 100
                };
                moduleItems.Add(new ListBoxItem() { Content = dragAndDropItem });
            }
            ModuleList.ItemsSource = moduleItems;


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
    }
}
