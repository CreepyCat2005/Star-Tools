using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Star_Citizen_Pfusch.Pages.SettingsFolder
{
    /// <summary>
    /// Interaction logic for AppearanceSettings.xaml
    /// </summary>
    public partial class AppearanceSettings : Page
    {
        private Timer timer;
        private int direction = -1;
        public AppearanceSettings()
        {
            this.Unloaded += AppearanceSettings_Unloaded;

            InitializeComponent();

            TextBox.SelectedValue = Application.Current.Resources["TextFontSize"].ToString();
            MenuBox.SelectedValue = Application.Current.Resources["MenuFontSize"].ToString();
            HeadlineBox.SelectedValue = Application.Current.Resources["HeadlineFontSize"].ToString();
        }

        private void AppearanceSettings_Unloaded(object sender, RoutedEventArgs e)
        {
            UserConfigItem userConfig = new UserConfigItem()
            {
                HeadlineColor = ((SolidColorBrush)Application.Current.Resources["HeadlineColor"]).Color.ToString(),
                MenuColor = ((SolidColorBrush)Application.Current.Resources["MenuColor"]).Color.ToString(),
                TextColor = ((SolidColorBrush)Application.Current.Resources["TextColor"]).Color.ToString(),
                Theme = (string)Application.Current.Resources["Theme"],
                TextFontSize = (double)Application.Current.Resources["TextFontSize"],
                MenuFontSize = (double)Application.Current.Resources["MenuFontSize"],
                HeadlineFontSize = (double)Application.Current.Resources["HeadlineFontSize"]
            };
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Config/UserConfig.cfg",JsonConvert.SerializeObject(userConfig));
        }
        private void ColorText_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["TextColor"] = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorMenu_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["MenuColor"] = new SolidColorBrush((Color)e.NewValue);
        }
        private void ColorHeadline_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue == Colors.Transparent) { ((ColorPicker)sender).SelectedColor = e.OldValue; return; }
            Application.Current.Resources["HeadlineColor"] = new SolidColorBrush((Color)e.NewValue);
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

        private void TextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.Resources["TextFontSize"] = double.Parse(((ComboBoxItem)((ComboBox)e.Source).SelectedItem).Content.ToString());
        }

        private void MenuBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.Resources["MenuFontSize"] = double.Parse(((ComboBoxItem)((ComboBox)e.Source).SelectedItem).Content.ToString());
        }

        private void HeadlineBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.Resources["HeadlineFontSize"] = double.Parse(((ComboBoxItem)((ComboBox)e.Source).SelectedItem).Content.ToString());
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (DarkCheckBox == null || WhiteCheckBox == null || RainbowCheckBox == null) return;
            List<CheckBox> checkBoxes = new List<CheckBox>(new CheckBox[] { DarkCheckBox, WhiteCheckBox, RainbowCheckBox });
            checkBoxes.Remove((CheckBox)sender);
            for (int i = 0; i < checkBoxes.Count; i++)
            {
                checkBoxes[i].IsChecked = false;
            }

            switch (checkBoxes.Find(o => o.IsChecked == true).Content)
            {
                case "White":
                    timer.Stop();
                    timer.Dispose();

                    break;
                case "Dark":
                    timer.Stop();
                    timer.Dispose();

                    break;
                case "Rainbow":
                    startRainbow();
                    break;
                default:
                    break;
            }
        }
        private async void startRainbow()
        {
            timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = 100;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
        }
    }
}
