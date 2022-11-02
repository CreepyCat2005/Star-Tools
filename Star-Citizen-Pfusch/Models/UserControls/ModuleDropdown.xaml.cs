using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for ModuleDropdown.xaml
    /// </summary>
    public partial class ModuleDropdown : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string moduleName;
        private ModuleItem moduleItem;
        private Popup popup;

        public bool Child { get; set; } = false;
        public int Index { get; set; }
        public int LoadOutIndex { get; set; }
        public new SolidColorBrush Background { get; set; }
        public new SolidColorBrush BorderBrush { get; set; }
        public CornerRadius CornerRadius { get; set; }
        public string ModuleName
        {
            get
            {
                return moduleName;
            }
            set
            {
                moduleName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModuleName)));
            }
        }
        public ModuleItem ModuleItem
        {
            get
            {
                return moduleItem;
            }
            set
            {
                moduleItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModuleItem)));
            }
        }
        public bool Editable { get; set; }
        public ModuleItem[] ModuleArray { get; set; }
        public ModuleDropdown()
        {
            InitializeComponent();
            this.DataContext = this;

            Loaded += init;
            
        }

        private void init(object sender, RoutedEventArgs e)
        {
            TypeImage.Source = new BitmapImage(new Uri($"/Graphics/{ModuleItem.Type}.png", UriKind.Relative));

            if (!Editable)
            {
                DropDownImage.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Graphics\\Icons\\Lock-Icon.png", UriKind.Absolute));
            }
        }

        private void ArrowButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Border button = (Border)sender;
            if (Editable)
            {
                button.Background = new SolidColorBrush(Colors.Gray);
            }
        }

        private void ArrowButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Border button = (Border)sender;
            button.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void ArrowButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ModuleListPopup moduleListPopup = new ModuleListPopup()
            {
                ModuleArray = ModuleArray.Where(o => o.Type.Equals(ModuleItem.Type) && o.Size.Equals(ModuleItem.Size)).ToArray(),
                ModuleDropdown = this
            };

            popup = new Popup()
            {
                AllowsTransparency = true,
                PlacementTarget = (Border)sender,
                StaysOpen = false,
                Effect = new DropShadowEffect()
                {
                    ShadowDepth = 2,
                    Opacity = .6
                },
                IsOpen = true,
                MaxHeight = 300,
                VerticalOffset = 10,
                Child = moduleListPopup
            };
        }
    }
}
