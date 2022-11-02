using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Star_Citizen_Pfusch.Pages.SettingsFolder
{
    /// <summary>
    /// Interaction logic for AppearanceSettings.xaml
    /// </summary>
    public partial class AppearanceSettings : Page
    {
        private Page ThisPage;
        private SolidColorBrush TextColor, MenuColor, HeadlineColor, SliderColor;

        public bool IsModernShipListActive
        {
            get
            {
                return Config.ModernShipList;
            }
            set
            {
                Config.ModernShipList = value;
            }
        }

        public AppearanceSettings()
        {
            ThisPage = this;

            InitializeComponent();
            this.DataContext = this;

            TextColor = (SolidColorBrush)Application.Current.Resources["TextColor"];
            MenuColor = (SolidColorBrush)Application.Current.Resources["MenuColor"];
            HeadlineColor = (SolidColorBrush)Application.Current.Resources["HeadlineColor"];
            SliderColor = (SolidColorBrush)Application.Current.Resources["SliderColor"];

            switch (Config.ChartResolution)
            {
                case 5:
                    LowCheckBox.IsChecked = true;
                    break;
                case 12:
                    MediumCheckBox.IsChecked = true;
                    break;
                case 20:
                    HighCheckBox.IsChecked = true;
                    break;
            }
        }
        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources[((ColorPicker)sender).Name] = new SolidColorBrush((Color)e.NewValue);
            ((ColorPicker)sender).Background = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorText_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["TextColor"] = new SolidColorBrush((Color)e.NewValue);
            ((ColorPicker)sender).Background = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorMenu_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["MenuColor"] = new SolidColorBrush((Color)e.NewValue);
            ((ColorPicker)sender).Background = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorHeadline_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["HeadlineColor"] = new SolidColorBrush((Color)e.NewValue);
            ((ColorPicker)sender).Background = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorChart_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["ChartColor"] = new SolidColorBrush((Color)e.NewValue);
            ((ColorPicker)sender).Background = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorChartPoint_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["ChartPointColor"] = new SolidColorBrush((Color)e.NewValue);
            ((ColorPicker)sender).Background = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorSlider_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["SliderColor"] = new SolidColorBrush((Color)e.NewValue);
            ((ColorPicker)sender).Background = new SolidColorBrush((Color)e.NewValue);
        }

        private void ColorText_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["TextColor"]).Color;
        }
        private void ColorMenu_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["MenuColor"]).Color;
        }
        private void ColorHeadline_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["HeadlineColor"]).Color;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();
            checkBoxes.Add(LowCheckBox);
            checkBoxes.Add(MediumCheckBox);
            checkBoxes.Add(HighCheckBox);
            checkBoxes.Add(OffCheckBox);
            checkBoxes.Remove((CheckBox)sender);

            foreach (var item in checkBoxes)
            {
                item.IsChecked = false;
            }

            if ((bool)LowCheckBox.IsChecked)
            {
                Config.ChartResolution = 5;
            }
            else if ((bool)MediumCheckBox.IsChecked)
            {
                Config.ChartResolution = 12;
            }
            else if ((bool)HighCheckBox.IsChecked)
            {
                Config.ChartResolution = 20;
            }
            else if ((bool)OffCheckBox.IsChecked)
            {
                Config.ChartResolution = 1;
            }
        }

        private void ColorChart_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["ChartColor"]).Color;
        }
        private void ColorChartPoint_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["ChartPointColor"]).Color;
        }
        private void ColorSlider_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["SliderColor"]).Color;
        }
    }
}
