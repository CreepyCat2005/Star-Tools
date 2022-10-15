using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.Enums;
using Star_Citizen_Pfusch.Models.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Core.AnimationMetrics;

namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ModernShipView.xaml
    /// </summary>
    public partial class ModernShipView : Page
    {
        private ModuleItem[] ModuleArray;
        private ShipItem shipItem;
        public ModernShipView(string shipID)
        {
            InitializeComponent();
            this.DataContext = this;

            init(shipID);
        }
        private void init(string shipID)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.GetAsync(Config.URL + $"/Ship?ID={shipID}").Result;
            string res = response.Content.ReadAsStringAsync().Result;

            shipItem = JsonConvert.DeserializeObject<ShipItem>(res);
            ShipImage.Source = new BitmapImage(new Uri("/Graphics/ShipImages/" + shipItem.LocalName + ".jpg", UriKind.Relative));


            response = client.GetAsync(Config.URL + "/Module/All").Result;
            res = response.Content.ReadAsStringAsync().Result;

            JArray jArray = JArray.Parse(res);
            List<ModuleItem> modules = new List<ModuleItem>();
            foreach (var item in jArray)
            {
                ModuleItem.DeserializeItem(item, modules);
            }
            ModuleArray = modules.ToArray();

            ModuleItem[] moduleArray = GetModuleItems("hardpoint", shipItem.Loadout, ModuleArray);

            foreach (var item in moduleArray.DistinctBy(o => o.LocalName))
            {
                ArrowFrame frame = new ArrowFrame()
                {
                    Source = new BitmapImage(new Uri("/Graphics/" + item.Type + ".png", UriKind.Relative)),
                    Margin = new Thickness(10, 0, 10, 0),
                    ModuleItems = moduleArray.Where(o => o.LocalName.Equals(item.LocalName)).ToArray(),
                    shipItem = shipItem,
                    ModuleArray = ModuleArray
                };
                frame.OnModuleRequest += Frame_OnModuleRequest;

                ItemStackPanel.Children.Add(frame);
            }

            TextBlock header = new TextBlock()
            {
                FontSize = 18,
                Text = "Info",
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            header.SetResourceReference(ForegroundProperty, "HeadlineColor");
            ShipInfoStackPanel.Children.Add(header);
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Name", shipItem.Name));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Manufacturer", shipItem.Manufacturer.Name));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Description", shipItem.Description));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Size", $"{shipItem.ShipSize.Length} x {shipItem.ShipSize.Width} x {shipItem.ShipSize.Height}"));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Role", shipItem.Role));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Career", shipItem.Career));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Size", shipItem.Size.ToString()));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Cargo", String.Format("{0:n0} SCU", shipItem.Cargo)));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Mass", String.Format("{0:n0}", shipItem.Mass)));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Hydrogen Fuel", String.Format("{0:n0}", shipItem.HydrogenFuelCapacity)));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Quantum Fuel", String.Format("{0:n0}", shipItem.QuantumFuelCapacity)));
            if (shipItem.Shops.Length > 0) ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Price", String.Format("{0:n0} aUEC", shipItem.Shops[0].price)));
            else ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Price", "Not buyable ingame!"));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Status", shipItem.Status.ToString()));
            TextBlock shops = new TextBlock()
            {
                FontSize = 18,
                Text = "\nShops",
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            shops.SetResourceReference(ForegroundProperty, "HeadlineColor");
            ShipInfoStackPanel.Children.Add(shops);
            foreach (var item in shipItem.Shops)
            {
                if (!item.name.Contains("Rental"))
                {
                    TextBlock text = new TextBlock()
                    {
                        FontSize = 18,
                        Text = $"{item.location} | {item.name} | {String.Format("{0:n0} aUEC", item.price)}",
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    text.SetResourceReference(ForegroundProperty, "TextColor");
                    ShipInfoStackPanel.Children.Add(text);
                }
                else
                {
                    TextBlock text = new TextBlock()
                    {
                        FontSize = 18,
                        Text = $"{item.location} | {item.name} | Depends on days",
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    text.SetResourceReference(ForegroundProperty, "TextColor");
                    ShipInfoStackPanel.Children.Add(text);
                }
            }

        }

        private void Frame_OnModuleRequest(object sender, ModuleRequestArgs e)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(Config.URL + $"/Module/Specific?_id={e.ModuleDropdown.ModuleItem._id}&type={e.ModuleDropdown.ModuleItem.Type}").Result;
            string res = response.Content.ReadAsStringAsync().Result;

            object item;
            switch (e.ModuleDropdown.ModuleItem.Type)
            {
                case "Shield":
                    item = JsonConvert.DeserializeObject<ShieldItem>(res);
                    break;
                case "PowerPlant":
                    item = JsonConvert.DeserializeObject<PowerPlantItem>(res);
                    break;
                case "Cooler":
                    item = JsonConvert.DeserializeObject<CoolerItem>(res);
                    break;
                case "QuantumDrive":
                    item = JsonConvert.DeserializeObject<QuantumDriveItem>(res);
                    break;
                case "WeaponGun":
                    item = JsonConvert.DeserializeObject<WeaponItem>(res);
                    break;
                case "Turret":
                    item = JsonConvert.DeserializeObject<MountingItem>(res);
                    break;
                default:
                    item = JsonConvert.DeserializeObject<ModuleItem>(res);
                    break;
            }
            ModuleInfoStackPanel.Children.Clear();

            SetStackPanelPropertiers(item);

            BackgroundBorderLeft.Visibility = Visibility.Visible;
        }
        private void SetStackPanelPropertiers(object item)
        {
            foreach (var prop in item.GetType().GetProperties())
            {
                bool aasds = prop.PropertyType != typeof(string) && prop.PropertyType != typeof(string[]) && prop.PropertyType.IsClass && prop.GetValue(item, null) != null;
                if (prop.PropertyType != typeof(string) && prop.PropertyType != typeof(string[]) && prop.PropertyType.IsClass && prop.GetValue(item, null) != null)
                {
                    SetStackPanelPropertiers(prop.GetValue(item, null));
                    continue;
                }
                var value = prop.GetValue(item, null);
                if (value == null || prop.Name.Equals("_id")) continue;
                if (value.GetType() == typeof(string[])) value = StringArrayToString((string[])value);
                ModuleInfoStackPanel.Children.Add(CreateStackPanelGrid(prop.Name, value.ToString(), 470));
            }
        }
        private Border CreateStackPanelGrid(string name, string value, int width = 500)
        {
            Grid grid = new Grid()
            {
                Margin = new Thickness(1, 3, 1, 3)
            };

            Border border = new Border()
            {
                Child = grid,
                Style = (Style)this.Resources["BorderStyle"]
            };

            TextBlock text1 = new TextBlock()
            {
                FontSize = 18,
                Text = name,
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = width / 2
            };
            text1.SetResourceReference(ForegroundProperty, "TextColor");
            grid.Children.Add(text1);
            TextBlock text2 = new TextBlock()
            {
                FontSize = 18,
                Text = value,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = width / 2,
                TextWrapping = TextWrapping.Wrap
            };
            text2.SetResourceReference(ForegroundProperty, "TextColor");
            grid.Children.Add(text2);

            return border;
        }


        private ModuleItem[] GetModuleItems(string itemPortName, LoadoutItem[] loadout, ModuleItem[] modules)
        {
            var loadOuTemp = loadout.Where(o => o.itemPortName.ToLower().Contains(itemPortName)).ToList();
            if (loadOuTemp.Count <= 0) return new ModuleItem[0];

            List<ModuleItem> moduleItems = new List<ModuleItem>();
            for (int i = 0; i < loadOuTemp.Count; i++)
            {
                string localName = loadOuTemp[i].localName;

                var items = modules.Where(o => o.LocalName.Equals(localName)).ToList();
                if (items.Count == 0) continue;
                var item = items[0];

                moduleItems.Add(item);
            }

            return moduleItems.ToArray();
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;

            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + e.Delta / 10);
        }
        private string StringArrayToString(string[] arr)
        {
            string output = "";
            foreach (var item in arr)
            {
                output += item + "\n";
            }
            return output.Substring(0, output.Length - 1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Content.Equals("Retracted"))
            {
                btn.RenderTransform = new RotateTransform()
                {
                    Angle = 180,
                    CenterX = 15,
                    CenterY = 15
                };

                DoubleAnimation doubleAnimation = new DoubleAnimation(30, 500, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

                BackgroundBorderRight.BeginAnimation(WidthProperty, doubleAnimation);
                btn.Content = "Extended";
            }
            else
            {
                btn.RenderTransform = new RotateTransform()
                {
                    Angle = 0,
                    CenterX = 15,
                    CenterY = 15
                };

                DoubleAnimation doubleAnimation = new DoubleAnimation(500, 30, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

                BackgroundBorderRight.BeginAnimation(WidthProperty, doubleAnimation);
                btn.Content = "Retracted";
            }
        }
        private void ModuleInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Content.Equals("Retracted"))
            {
                btn.RenderTransform = new RotateTransform()
                {
                    Angle = 180,
                    CenterX = 15,
                    CenterY = 15
                };

                DoubleAnimation doubleAnimation = new DoubleAnimation(30, 500, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

                BackgroundBorderLeft.BeginAnimation(WidthProperty, doubleAnimation);
                btn.Content = "Extended";
            }
            else
            {
                btn.RenderTransform = new RotateTransform()
                {
                    Angle = 0,
                    CenterX = 15,
                    CenterY = 15
                };

                DoubleAnimation doubleAnimation = new DoubleAnimation(500, 30, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

                BackgroundBorderLeft.BeginAnimation(WidthProperty, doubleAnimation);
                btn.Content = "Retracted";
            }
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            BackgroundBorderRight.Visibility = Visibility.Visible;
            BackgroundBorderTop.Visibility = Visibility.Visible;
            if (ModuleInfoStackPanel.Children.Count > 0) BackgroundBorderLeft.Visibility = Visibility.Visible;
            BackgroundBorderRight.BeginAnimation(OpacityProperty, doubleAnimation);
            BackgroundBorderTop.BeginAnimation(OpacityProperty, doubleAnimation);
            BackgroundBorderLeft.BeginAnimation(OpacityProperty, doubleAnimation);

            ShowButton.Visibility = Visibility.Visible;
            HideButton.Visibility = Visibility.Hidden;
        }

        private async void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            BackgroundBorderRight.BeginAnimation(OpacityProperty, doubleAnimation);
            BackgroundBorderTop.BeginAnimation(OpacityProperty, doubleAnimation);
            BackgroundBorderLeft.BeginAnimation(OpacityProperty, doubleAnimation);

            ShowButton.Visibility = Visibility.Hidden;
            HideButton.Visibility = Visibility.Visible;

            await Task.Delay(500);

            BackgroundBorderRight.Visibility = Visibility.Hidden;
            BackgroundBorderLeft.Visibility = Visibility.Hidden;
            BackgroundBorderTop.Visibility = Visibility.Hidden;
        }
    }
}