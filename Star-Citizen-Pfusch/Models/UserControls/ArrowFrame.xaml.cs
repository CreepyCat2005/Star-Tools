using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for ArrowFrame.xaml
    /// </summary>
    public partial class ArrowFrame : UserControl
    {
        public delegate void ModuleRequestHandler(object sender, ModuleRequestArgs e);
        public event ModuleRequestHandler OnModuleRequest;
        private List<ModuleItem> ModuleLoadoutItem = new List<ModuleItem>();

        public ImageSource Source { get; set; }
        public ModuleItem[] ModuleItems { get; set; }
        public ModuleItem[] ModuleArray { get; set; }
        public ShipItem shipItem { get; set; }
        public ArrowFrame()
        {
            InitializeComponent();
            this.DataContext = this;

            Loaded += ArrowFrame_Loaded;
        }

        private void ArrowFrame_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in ModuleItems)
            {
                LoadoutItem loadout = GetLowestContainer(shipItem.Loadout.Where(o => o.localName.Equals(item.LocalName)).First());
                List<ModuleItem> moduleItems = new List<ModuleItem>();

                for (int i = 0; i < loadout.Loadout.Length; i++)
                {
                    moduleItems.Add(ModuleArray.Where(o => o.LocalName.Equals(loadout.Loadout[i].localName)).First());
                }

                item.Loadout = moduleItems.ToArray();
                ModuleLoadoutItem.Add((ModuleItem)item.Clone());
            }

            ModuleArray = ModuleArray.Where(o => ArrayContainsArray(shipItem.Tags, o.RequiredTags)).ToArray();
        }
        private bool ArrayContainsArray(string[] baseArray, string[] checkArray)
        {
            if (checkArray == null) return true;
            bool value = true;
            for (int i = 0; i < checkArray.Length; i++)
            {
                if (!baseArray.Contains(checkArray[i])) value = false;
            }

            return value;
        }

        private void Frame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RenderPopup();
        }
        private void RenderPopup()
        {
            StackPanel stackPanel = new StackPanel();

            for (int x = 0; x < ModuleLoadoutItem.Count; x++)
            {
                ModuleDropdown dropdown = new ModuleDropdown()
                {
                    CornerRadius = new CornerRadius(10),
                    ModuleName = ModuleLoadoutItem[x].Name,
                    Height = 50,
                    Background = (SolidColorBrush)Resources["ItemBackground"],
                    BorderBrush = (SolidColorBrush)Resources["ItemStroke"],
                    ModuleItem = ModuleLoadoutItem[x],
                    Editable = true,
                    ModuleArray = ModuleArray,
                    Index = x
                };
                dropdown.Unloaded += Dropdown_Unloaded;
                dropdown.MouseDoubleClick += ModuleDropdown_MouseDoubleClick;

                stackPanel.Children.Add(dropdown);

                if (ModuleLoadoutItem[x].Loadout == null) continue;
                for (int i = 0; i < ModuleLoadoutItem[x].Loadout.Length; i++)
                {
                    ModuleItem module = ModuleLoadoutItem[x].Loadout[i];

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
                        ModuleItem = module,
                        Editable = true,
                        ModuleArray = ModuleArray,
                        Index = x,
                        LoadOutIndex = i,
                    };
                    moduleDropdown.Unloaded += Dropdown_Unloaded;
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
                PlacementTarget = this,
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

        private void Dropdown_Unloaded(object sender, RoutedEventArgs e)
        {
            ModuleDropdown moduleDropdown = (ModuleDropdown)sender;

            if (!moduleDropdown.ModuleItem.Type.Equals("Missile") && !moduleDropdown.ModuleItem.Type.Equals("WeaponGun"))
            {
                ModuleLoadoutItem[moduleDropdown.Index] = moduleDropdown.ModuleItem;
            }
            else
            {
                if (ModuleLoadoutItem[moduleDropdown.Index].Loadout.Length <= moduleDropdown.LoadOutIndex) return;
                ModuleLoadoutItem[moduleDropdown.Index].Loadout[moduleDropdown.LoadOutIndex] = moduleDropdown.ModuleItem;
            }
        }

        private void ModuleDropdown_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnModuleRequest?.Invoke(this, new ModuleRequestArgs((ModuleDropdown)sender));
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
    }
    public class ModuleRequestArgs : EventArgs
    {
        public ModuleDropdown ModuleDropdown { get; set; }
        public ModuleRequestArgs(ModuleDropdown moduleDropdown)
        {
            ModuleDropdown = moduleDropdown;
        }
    }
}
