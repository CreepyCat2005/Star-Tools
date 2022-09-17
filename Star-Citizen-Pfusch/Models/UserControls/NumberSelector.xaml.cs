using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public int Value
        {
            get
            {
                return Properties.Value;
            }
            set
            {
                Properties.Value = value;
            }
        }
        public int MinValue
        {
            get
            {
                return Properties.MinValue;
            }
            set
            {
                Properties.MinValue = value;
            }
        }
        public int MaxValue
        {
            get
            {
                return Properties.MaxValue;
            }
            set
            {
                Properties.MaxValue = value;
            }
        }
        public double MaskOpacity { get; set; }
        public Brush ArrowColor { get; set; } = new SolidColorBrush(Colors.White);
        public delegate void ValueChangedHandler(object sender, ValueEventArgs e);
        public event ValueChangedHandler ValueChanged;

        public NumberSelector()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Left_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Value <= Properties.MinValue) return;
            Properties.Value--;
            if (ValueChanged != null)
            {
                ValueEventArgs args = new ValueEventArgs(Properties.Value);
                ValueChanged(this, args);
            }
        }
        private void Right_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Value >= Properties.MaxValue) return;
            Properties.Value++;
            if (ValueChanged != null)
            {
                ValueEventArgs args = new ValueEventArgs(Properties.Value);
                ValueChanged(this, args);
            }
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && !(Properties.Value >= Properties.MaxValue))
            {
                Properties.Value++;
                if (ValueChanged != null)
                {
                    ValueEventArgs args = new ValueEventArgs(Properties.Value);
                    ValueChanged(this, args);
                }
            }
            else if (e.Delta < 0 && !(Properties.Value <= Properties.MinValue))
            {
                Properties.Value--;
                if (ValueChanged != null)
                {
                    ValueEventArgs args = new ValueEventArgs(Properties.Value);
                    ValueChanged(this, args);
                }
            }
        }
    }
    public class NumberSelectorProperties : INotifyPropertyChanged
    {
        private int valueValue;
        private int maxValue;
        private int minValue;
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
        public int MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                NotifyPropertyChanged();
            }
        }
        public int MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ValueEventArgs : EventArgs
    {
        public int Value { get; private set; }
        public ValueEventArgs(int value)
        {
            Value = value;
        }
    }
}
