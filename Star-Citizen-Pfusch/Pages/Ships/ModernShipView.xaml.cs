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
                modules.Add(JsonConvert.DeserializeObject<ModuleItem>(item.ToString()));
            }
            ModuleArray = modules.ToArray();

            ModuleItem[] moduleArray = GetModuleItems("hardpoint", shipItem.Loadout, ModuleArray);

            foreach (var item in moduleArray.DistinctBy(o => o.LocalName))
            {
                ArrowFrame frame = new ArrowFrame()
                {
                    Source = new BitmapImage(new Uri("/Graphics/" + item.Type + ".png", UriKind.Relative)),
                    Margin = new Thickness(10, 0, 10, 0),
                    ModuleItems = moduleArray.Where(o => o.LocalName.Equals(item.LocalName)).ToArray()
                };
                frame.MouseLeftButtonUp += Frame_MouseLeftButtonUp;

                ItemStackPanel.Children.Add(frame);
            }

            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Name", shipItem.Name));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Manufacturer", shipItem.Manufacturer.Name));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Description", shipItem.Description));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Size", $"{shipItem.ShipSize.Length} x {shipItem.ShipSize.Width} x {shipItem.ShipSize.Height}"));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Role", shipItem.Role));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Career", shipItem.Career));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Size", shipItem.Size.ToString()));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Cargo", shipItem.Cargo.ToString()));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Mass", shipItem.Mass.ToString()));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Hydrogen Fuel", shipItem.HydrogenFuelCapacity.ToString()));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Quantum Fuel", shipItem.QuantumFuelCapacity.ToString()));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Price", shipItem.Price + " aUEC"));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Real Price", shipItem.RealPrice + "€"));
            ShipInfoStackPanel.Children.Add(CreateStackPanelGrid("Status", shipItem.Status.ToString()));

        }
        private Border CreateStackPanelGrid(string name, string value, int width = 370)
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

            grid.Children.Add(new TextBlock()
            {
                FontSize = 18,
                Text = name,
                HorizontalAlignment = HorizontalAlignment.Left,
                Foreground = new SolidColorBrush(Colors.White),
                Width = width / 2
            });
            grid.Children.Add(new TextBlock()
            {
                FontSize = 18,
                Text = value,
                HorizontalAlignment = HorizontalAlignment.Right,
                Foreground = new SolidColorBrush(Colors.White),
                Width = width / 2,
                TextWrapping = TextWrapping.Wrap
            });

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
        private void Frame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = new StackPanel();
            foreach (var item in ((ArrowFrame)sender).ModuleItems)
            {
                LoadoutItem loadout = GetLowestContainer(shipItem.Loadout.Where(o => o.localName.Equals(item.LocalName)).First());
                ModuleItem moduleContainer = ModuleArray.Where(o => o.LocalName.Equals(loadout.localName)).First();

                ModuleDropdown dropdown = new ModuleDropdown()
                {
                    CornerRadius = new CornerRadius(10),
                    ModuleName = moduleContainer.Name,
                    Height = 50,
                    Background = (SolidColorBrush)Resources["ItemBackground"],
                    BorderBrush = (SolidColorBrush)Resources["ItemStroke"],
                    ModuleItem = moduleContainer
                };
                dropdown.MouseDoubleClick += ModuleDropdown_MouseDoubleClick;

                stackPanel.Children.Add(dropdown);

                for (int i = 0; i < loadout.Loadout.Length; i++)
                {
                    var moduleArray = ModuleArray.Where(o => o.LocalName.Equals(loadout.Loadout[i].localName));
                    if (moduleArray.Count() == 0) continue;
                    ModuleItem module = moduleArray.First();

                    Image image = new Image()
                    {
                        Source = new BitmapImage(new Uri("/Graphics/ArrowRightDown.png", UriKind.Relative)),
                        Stretch = Stretch.Uniform,
                        Margin = new Thickness(5)
                    };
                    Grid.SetColumn(image, 0);

                    ModuleDropdown moduleDropdown = new ModuleDropdown()
                    {
                        CornerRadius = new CornerRadius(10),
                        ModuleName = module.Name,
                        Height = 50,
                        Background = (SolidColorBrush)Resources["ItemBackground"],
                        BorderBrush = (SolidColorBrush)Resources["ItemStroke"],
                        ModuleItem = module
                    };
                    moduleDropdown.MouseDoubleClick += ModuleDropdown_MouseDoubleClick;
                    Grid.SetColumn(moduleDropdown, 1);

                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(moduleDropdown.Height) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.Children.Add(image);
                    grid.Children.Add(moduleDropdown);

                    stackPanel.Children.Add(grid);

                }
            }

            Popup popup = new Popup()
            {
                AllowsTransparency = true,
                PlacementTarget = (ArrowFrame)sender,
                StaysOpen = false,
                Effect = new DropShadowEffect()
                {
                    ShadowDepth = 2,
                    Opacity = .6
                },
                IsOpen = true,
                VerticalOffset = 10,
                Child = new Border()
                {
                    BorderBrush = (SolidColorBrush)Resources["ItemStroke"],
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Background = (SolidColorBrush)Resources["ItemBackground"],
                    Child = stackPanel,
                    Effect = new DropShadowEffect()
                    {
                        ShadowDepth = 0,
                        Opacity = .6
                    }
                }
            };
        }

        private void ModuleDropdown_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(Config.URL + $"/Module/Specific?_id={((ModuleDropdown)sender).ModuleItem._id}&type={((ModuleDropdown)sender).ModuleItem.Type}").Result;
            string res = response.Content.ReadAsStringAsync().Result;

            object item;
            switch (((ModuleDropdown)sender).ModuleItem.Type)
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
                default:
                    item = ((ModuleDropdown)sender).ModuleItem;
                    break;
            }
            ModuleInfoStackPanel.Children.Clear();

            foreach (var prop in item.GetType().GetProperties())
            {
                var value = prop.GetValue(item, null);
                if (value == null) continue;
                if (value.GetType() == typeof(string[])) value = StringArrayToString((string[])value);
                ModuleInfoStackPanel.Children.Add(CreateStackPanelGrid(prop.Name, value.ToString(), 470));
            }

            //ModuleInfoStackPanel.Children.Add(CreateStackPanelGrid("Description", item.Description));
            //ModuleInfoStackPanel.Children.Add(CreateStackPanelGrid("Size", item.Size.ToString()));
            //ModuleInfoStackPanel.Children.Add(CreateStackPanelGrid("Grade", item.Grade.ToString()));
            //ModuleInfoStackPanel.Children.Add(CreateStackPanelGrid("Health", item.Health.ToString()));
            //ModuleInfoStackPanel.Children.Add(CreateStackPanelGrid("Mass", item.Mass.ToString()));

            BackgroundBorderLeft.Visibility = Visibility.Visible;
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

        private LoadoutItem GetLowestContainer(LoadoutItem loadout)
        {
            if (loadout.Loadout != null && loadout.Loadout.Length > 0 && loadout.Loadout[0].Loadout != null)
            {
                return GetLowestContainer(loadout.Loadout[0]);
            }
            else
            {
                if (loadout.Loadout == null) loadout.Loadout = new LoadoutItem[0];
                return loadout;
            }
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

                DoubleAnimation doubleAnimation = new DoubleAnimation(30,400,new Duration(new TimeSpan(0, 0, 0, 0, 200)));

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

                DoubleAnimation doubleAnimation = new DoubleAnimation(400, 30, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

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