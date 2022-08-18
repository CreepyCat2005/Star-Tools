using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Page ThisPage;
        private Timer timer;
        private double counter = 0;

        public AppearanceSettings()
        {
            ThisPage = this;
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
            checkBoxes.Add((CheckBox)sender);

            switch (checkBoxes.Find(o => o.IsChecked == true).Content)
            {
                case "White":
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    Application.Current.Resources["TextColor"] = new SolidColorBrush(Colors.White);
                    Application.Current.Resources["MenuColor"] = new SolidColorBrush(Colors.White);
                    Application.Current.Resources["HeadlineColor"] = new SolidColorBrush(Colors.White);
                    break;
                case "Dark":
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    Application.Current.Resources["TextColor"] = new SolidColorBrush(Colors.White);
                    Application.Current.Resources["MenuColor"] = new SolidColorBrush(Colors.White);
                    Application.Current.Resources["HeadlineColor"] = new SolidColorBrush(Colors.White);
                    break;
                case "Rainbow":
                    StartRainbow();
                    break;
                default:
                    break;
            }
        }
        private async void StartRainbow()
        {
            timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = 35;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Resources["TextColor"] = new SolidColorBrush(ColorFromHSV(counter,1,1));
            Application.Current.Resources["MenuColor"] = new SolidColorBrush(ColorFromHSV(counter,1,1));
            Application.Current.Resources["HeadlineColor"] = new SolidColorBrush(ColorFromHSV(counter,1,1));
            double temp = 0;
            ThisPage.Dispatcher.Invoke(new Action(() =>
            {
                temp = RainbowSpeed.Value;
            }));
            counter += temp;
            Debug.WriteLine("Color assigned: " + ColorFromHSV(counter, 1, 1).R + " " + ColorFromHSV(counter, 1, 1).G + " " + ColorFromHSV(counter, 1, 1).B);
        }
        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            byte v = Convert.ToByte(value);
            byte p = Convert.ToByte(value * (1 - saturation));
            byte q = Convert.ToByte(value * (1 - f * saturation));
            byte t = Convert.ToByte(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
