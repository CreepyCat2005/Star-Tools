using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for NumberSelector.xaml
    /// </summary>
    public partial class NumberSelector : UserControl
    {
        public NumberSelectorProperties Properties { get; } = new NumberSelectorProperties();
        public int Value { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public double MaskOpacity { get; set; }
        public Brush ArrowColor { get; set; } = new SolidColorBrush(Colors.White);

        public NumberSelector()
        {
            InitializeComponent();
            this.DataContext = this;
            Loaded += NumberSelector_Loaded;
        }

        private void NumberSelector_Loaded(object sender, RoutedEventArgs e)
        {
            Properties.Value = Value;
        }

        private void Left_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Value <= MinValue) return;
            Properties.Value--;
        }
        private void Right_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Value >= MaxValue) return;
            Properties.Value++;
        }
    }
    public class NumberSelectorProperties : INotifyPropertyChanged
    {
        private int valueValue;
        public int Value
        {
            get
            {
                return valueValue;
            }
            set
            {
                valueValue = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
