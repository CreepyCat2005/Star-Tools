using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
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
using System.Text;
using System.Windows.Controls.Primitives;
using Star_Citizen_Pfusch.Models.UserControls;
using Star_Citizen_Pfusch.Animations.Symbols;
using System.Windows.Navigation;

namespace Star_Citizen_Pfusch.Pages.Ships
{
    /// <summary>
    /// Interaction logic for ShipInformation.xaml
    /// </summary>
    public partial class ShipView : Page
    {
        private ObservableCollection<ListBoxItem> moduleItems = new ObservableCollection<ListBoxItem>();
        private List<ListBoxItem> moduleTargetItems = new List<ListBoxItem>();
        private FilterSettings filterSettings = new FilterSettings();
        private ShipStatistics shipStatistics;
        private ModuleStatistics moduleStatistics;
        private ShipItem shipItem;
        private ModuleItem[] ModuleArray;
        private List<ModuleInfoItem> moduleInfoItems = new List<ModuleInfoItem>();

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
            var asd = moduleInfoItems.DistinctBy(o => new { o.MinSize, o.MaxSize, o.Type }).ToList();

            var items = ModuleArray.Where(o => asd.Select(o => o.Type).Contains(o.Type) && asd.Where(x => x.MinSize <= o.Size && x.Type.Equals(o.Type)).Count() != 0 && asd.Where(x => x.MaxSize >= o.Size && x.Type.Equals(o.Type)).Count() != 0);

            foreach (var module in items)
            {
                DragAndDropItem dragAndDropItem = new DragAndDropItem()
                {
                    _id = module._id,
                    moduleItem = module,
                    QtNameText = module.Name,
                    QtGradeText = "Grade: " + module.Grade,
                    QtSizeText = "Size: " + module.Size,
                    Size = module.Size,
                    Type = module.Type
                };

                ListBoxItem boxItem = new ListBoxItem() { Content = dragAndDropItem };
                boxItem.SizeChanged += BoxItem_SizeChanged;
                boxItem.MouseEnter += BoxItem_MouseEnter;
                boxItem.MouseLeave += BoxItem_MouseLeave;
                boxItem.MouseDoubleClick += BoxItem_MouseDoubleClick;

                moduleItems.Add(boxItem);
            }
            ModuleList.ItemsSource = moduleItems;
        }

        private void BoxItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)sender;
            item.Height = item.ActualWidth / 1.6;
        }
        private void filterModules(object sender, RoutedEventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            List<ListBoxItem> items = new List<ListBoxItem>();
            List<QuantumDriveItem> quantumDriveList;
            List<ShieldItem> shieldList;
            ListBoxItem listBox;

            switch (box.Content.ToString())
            {
                case "Speed":
                    quantumDriveList = moduleItems.OfType<ListBoxItem>().Select(o => o.Content).OfType<DragAndDropItem>().Select(o => o.moduleItem).OfType<QuantumDriveItem>().OrderByDescending(o => o.driveSpeed).ToList();
                    foreach (var item in quantumDriveList)
                    {
                        DragAndDropItem dragAndDropItem = new DragAndDropItem()
                        {
                            _id = item._id,
                            moduleItem = item,
                            QtNameText = item.Name,
                            QtGradeText = "Grade: " + item.Grade,
                            QtSizeText = "Size: " + item.Size,
                            Size = item.Size,
                            Type = item.Type
                        };

                        listBox = new ListBoxItem();
                        listBox.MouseEnter += BoxItem_MouseEnter;
                        listBox.MouseLeave += BoxItem_MouseLeave;
                        listBox.MouseDoubleClick += BoxItem_MouseDoubleClick;
                        listBox.SizeChanged += BoxItem_SizeChanged;
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
                    quantumDriveList = moduleItems.OfType<ListBoxItem>().Select(o => o.Content).OfType<DragAndDropItem>().Select(o => o.moduleItem).OfType<QuantumDriveItem>().OrderBy(o => o.quantumFuelRequirement).ToList();
                    foreach (var item in quantumDriveList)
                    {
                        DragAndDropItem dragAndDropItem = new DragAndDropItem()
                        {
                            _id = item._id,
                            moduleItem = item,
                            QtNameText = item.Name,
                            QtGradeText = "Grade: " + item.Grade,
                            QtSizeText = "Size: " + item.Size,
                            Size = item.Size,
                            Type = item.Type
                        };

                        listBox = new ListBoxItem();
                        listBox.MouseEnter += BoxItem_MouseEnter;
                        listBox.MouseLeave += BoxItem_MouseLeave;
                        listBox.MouseDoubleClick += BoxItem_MouseDoubleClick;
                        listBox.SizeChanged += BoxItem_SizeChanged;
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
                    shieldList = moduleItems.OfType<ListBoxItem>().Select(o => o.Content).OfType<DragAndDropItem>().Select(o => o.moduleItem).OfType<ShieldItem>().OrderByDescending(o => o.MaxShieldHealth).ToList();
                    foreach (var item in shieldList)
                    {
                        DragAndDropItem dragAndDropItem = new DragAndDropItem()
                        {
                            _id = item._id,
                            moduleItem = item,
                            QtNameText = item.Name,
                            QtGradeText = "Grade: " + item.Grade,
                            QtSizeText = "Size: " + item.Size,
                            Size = item.Size,
                            Type = item.Type
                        };

                        listBox = new ListBoxItem();
                        listBox.MouseEnter += BoxItem_MouseEnter;
                        listBox.MouseLeave += BoxItem_MouseLeave;
                        listBox.MouseDoubleClick += BoxItem_MouseDoubleClick;
                        listBox.SizeChanged += BoxItem_SizeChanged;
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
        //! HOW TF DOES THIS WORK?????
        private void sortModules(CheckBox item)
        {
            for (int i = 0; i < moduleItems.Count; i++)
            {
                DragAndDropItem dragAndDropItem = (DragAndDropItem)moduleItems[i].Content;

                if (dragAndDropItem.moduleItem.Type.Equals(item.Content.ToString().Replace(" ", "")) || item.Content.ToString().Equals("All"))
                {
                    moduleItems[i].Visibility = Visibility.Visible;
                }
                else
                {
                    moduleItems[i].Visibility = Visibility.Collapsed;
                }

            }
        }
        //TODO: FIX THIS PEACE OF BULLSHIT!
        private void init(string shipID)
        {
            HttpClient client = new HttpClient();

            //getting ship data from server
            HttpResponseMessage response = client.GetAsync(Config.URL + $"/Ship?ID={shipID}").Result;
            string res = response.Content.ReadAsStringAsync().Result;

            shipItem = JsonConvert.DeserializeObject<ShipItem>(res);
            //end

            //!Setting basic data for shipview
            ShipImage.Source = new BitmapImage(new Uri("/Graphics/ShipImages/Small/" + shipItem.LocalName + ".jpg", UriKind.Relative));
            ShipStatus.Content = shipItem.Status;

            shipStatistics = new ShipStatistics()
            {
                ShipName = shipItem.Name,
                Role = shipItem.Role,
                Size = $"{shipItem.ShipSize.Length}m x {shipItem.ShipSize.Width}m x {shipItem.ShipSize.Height}m",
                Mass = shipItem.Mass.ToString(),
                Career = shipItem.Career,
                Description = shipItem.Description,
                Cargo = shipItem.Description.ToString()
            };

            //getting modules from server
            response = client.GetAsync(Config.URL + "/Module/All").Result;
            res = response.Content.ReadAsStringAsync().Result;

            JArray jArray = JArray.Parse(res);
            List<ModuleItem> modules = new List<ModuleItem>();
            foreach (var item in jArray)
            {
                modules.Add(JsonConvert.DeserializeObject<ModuleItem>(item.ToString()));
            }
            ModuleArray = modules.ToArray();
            //end

            List<ModuleItem> moduleItems = new List<ModuleItem>();
            moduleItems.AddRange(GetModuleItems("hardpoint", shipItem.Loadout, ModuleArray));

            foreach (var item in CreateListBoxItems(moduleItems, shipItem.Loadout.ToList()))
            {
                moduleTargetItems.Add(item);
            }

            ModuleTargetListBox.ItemsSource = moduleTargetItems;
        }
        private List<ListBoxItem> CreateListBoxItems(List<ModuleItem> modules, List<LoadoutItem> loadout)
        {
            var distributedIntegers = DistributeInteger(modules.Count, (int)Math.Ceiling(modules.Count / 8.0)).ToList();
            List<ListBoxItem> output = new List<ListBoxItem>();
            int offset = 0;

            for (int x = 0; x < distributedIntegers.Count; x++)
            {
                if (x - 1 >= 0)
                {
                    offset += distributedIntegers[x - 1];
                }

                ListBoxItem listBoxItem = new ListBoxItem()
                {
                    Height = 200,
                    Template = (ControlTemplate)Resources["ControlTemplate"],
                    Background = new SolidColorBrush(Colors.Transparent)
                };
                Grid grid = new Grid();

                for (int y = 0; y < distributedIntegers[x]; y++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    DragAndDropTarget target = new DragAndDropTarget()
                    {
                        Size = modules[offset + y].Size,
                        type = modules[offset + y].Type,
                        SubType = modules[offset + y].SubType
                    };

                    if (HasBody(modules[offset + y].Type))
                    {
                        LoadoutItem loadoutItem = loadout.Where(o => o.localName.Contains(modules[offset + y].LocalName)).ToList()[0];
                        ModuleItem subItem = GetSubItem(loadoutItem);
                        DragAndDropFrame frame;
                        if (modules[offset + y].SubType.Equals("MissileTurret"))
                        {
                            frame = FillBodyWithFrame(ModuleArray.Where(o => o.LocalName.Contains(loadoutItem.Loadout[0].localName)).First(), loadoutItem.Loadout[0], subItem);
                            moduleInfoItems.Add(new ModuleInfoItem()
                            {
                                Type = subItem.Type,
                                MinSize = subItem.Size,
                                MaxSize = subItem.Size
                            });
                        }
                        else
                        {
                            frame = FillBodyWithFrame(modules[offset + y], loadoutItem, subItem);
                            moduleInfoItems.Add(new ModuleInfoItem()
                            {
                                Type = subItem.Type,
                                MinSize = subItem.Size,
                                MaxSize = modules[offset + y].Size
                            });
                        }
                        loadout.Remove(loadoutItem);

                        target.ContentFrame.Content = frame;
                    }
                    else
                    {
                        target.ContentFrame.Content = new DragAndDropItem()
                        {
                            Type = modules[offset + y].Type,
                            QtNameText = modules[offset + y].Name,
                            QtGradeText = "Grade: " + modules[offset + y].Grade.ToString(),
                            QtSizeText = "Size: " + modules[offset + y].Size.ToString(),
                            Size = modules[offset + y].Size
                        };

                        moduleInfoItems.Add(new ModuleInfoItem()
                        {
                            Type = modules[offset + y].Type,
                            MinSize = modules[offset + y].Size,
                            MaxSize = modules[offset + y].Size
                        });
                    }

                    Grid.SetColumn(target,y);
                    grid.Children.Add(target);
                }
                listBoxItem.Content = grid;
                output.Add(listBoxItem);
            }
            return output;
        }
        private ModuleItem GetSubItem(LoadoutItem loadout)
        {
            if (loadout.Loadout != null)
            {
                return GetSubItem(loadout.Loadout[0]);
            }
            else
            {
                return ModuleArray.Where(o => o.LocalName.Equals(loadout.localName)).First();
            }
        }
        private void SetContentOfFrame(DragAndDropItem item, DragAndDropFrame frame)
        {
            if (frame.ContentFrame.Content != null)
            {
                SetContentOfFrame(item, (DragAndDropFrame)frame.ContentFrame.Content);
            }
            else
            {
                frame.ContentFrame.Content = item;
            }
        }
        private DragAndDropFrame FillBodyWithFrame(ModuleItem moduleItem, LoadoutItem loadout, ModuleItem subItem)
        {
            DragAndDropFrame frame = new DragAndDropFrame()
            {
                _id = moduleItem._id,
                ModuleName = moduleItem.Name,
                Size = moduleItem.Size,
                Type = moduleItem.Type
            };

            if (loadout.Loadout != null && loadout.Loadout[0].Loadout != null)
            {
                frame.ContentFrame.Content = FillBodyWithFrame(ModuleArray.Where(o => o.LocalName.Contains(loadout.Loadout[0].localName)).FirstOrDefault(), loadout.Loadout[0], subItem);
                return frame;
            }
            else
            {
                frame.ContentFrame.Content = new DragAndDropItem()
                {
                    _id = subItem._id,
                    Size = subItem.Size,
                    moduleItem = subItem,
                    Type = subItem.Type,
                    QtGradeText = "Anzahl: " + loadout.Loadout.Length,
                    QtNameText = subItem.Name,
                    QtSizeText = "Size: " + subItem.Size
                };

                return frame;
            }
        }
        private bool HasBody(string type)
        {
            if (type.Equals("MissileLauncher") || type.Equals("Turret") || type.Equals("TurretBase"))
            {
                return true;
            }
            else
            {
                return false;
            }
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
            moduleStatistics = new ModuleStatistics(item._id, item.Type, shipItem);
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
                if (!dragAndDropItem.QtNameText.ToLower().Contains(boxText.ToLower()))
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
                box.Foreground = (SolidColorBrush)Application.Current.Resources["TextColor"];
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
        private async void ShipWatcherButton_Click(object sender, RoutedEventArgs e)
        {
            MenuStackPanel.Children[0].Visibility = Visibility.Collapsed;

            LoadingSymbol loadingSymbol = new LoadingSymbol()
            {
                Margin = new Thickness(0, 0, 5, 0),
                Width = MenuStackPanel.ActualHeight * 0.8,
                Height = MenuStackPanel.ActualHeight * 0.8,
                CenterX = (int)(MenuStackPanel.ActualHeight * 0.8 / 2),
                CenterY = (int)(MenuStackPanel.ActualHeight * 0.8 / 2),
                HoleSize = new DoubleCollection(1) { 6 }
            };
            MenuStackPanel.Children.Insert(0, loadingSymbol);

            List<ShipWatcherItem> shipWatcherItems = new List<ShipWatcherItem>();
            shipWatcherItems.Add(new ShipWatcherItem() { localName = shipItem.LocalName, name = shipItem.Name});

            HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(new AccountDataItem() { SessionToken = Config.SessionToken, ShipsOnWatcher = shipWatcherItems }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(Config.URL + "/AccountData/ShipWatcher", content);

            MenuStackPanel.Children.Remove(loadingSymbol);

            if (response.IsSuccessStatusCode)
            {
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri("/Graphics/Icons/Check-Icon.png", UriKind.Relative)),
                    Margin = new Thickness(2, 2, 5, 2)
                };

                MenuStackPanel.Children.Insert(0, image);
                await Task.Delay(2000);
                MenuStackPanel.Children.Remove(image);
            }
            else
            {
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri("/Graphics/Icons/Exit-Icon.png", UriKind.Relative)),
                    Margin = new Thickness(2, 2, 5, 2)
                };

                MenuStackPanel.Children.Insert(0, image);
                await Task.Delay(2000);
                MenuStackPanel.Children.Remove(image);
            }
            MenuStackPanel.Children[0].Visibility = Visibility.Visible;
        }
    }
}
