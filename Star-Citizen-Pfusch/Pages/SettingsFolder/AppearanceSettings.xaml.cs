using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Pages.Ships;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private SolidColorBrush TextColor, MenuColor, HeadlineColor, SliderColor;

        public AppearanceSettings()
        {
            ThisPage = this;

            InitializeComponent();
            LoadTreeViewItems();

            this.Unloaded += AppearanceSettings_Unloaded;

            TextColor = (SolidColorBrush)Application.Current.Resources["TextColor"];
            MenuColor = (SolidColorBrush)Application.Current.Resources["MenuColor"];
            HeadlineColor = (SolidColorBrush)Application.Current.Resources["HeadlineColor"];
            SliderColor = (SolidColorBrush)Application.Current.Resources["SliderColor"];

            TextBox.SelectedValue = Application.Current.Resources["TextFontSize"].ToString();
            MenuBox.SelectedValue = Application.Current.Resources["MenuFontSize"].ToString();
            HeadlineBox.SelectedValue = Application.Current.Resources["HeadlineFontSize"].ToString();
        }
        private void AppearanceSettings_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["RainbowModeActive"] = RainbowCheckBox.IsChecked;
            Application.Current.Resources["RainbowModeValue"] = RainbowSpeed.Value;
        }
        private void LoadTreeViewItems()
        {
            foreach (var item in ((GridView)new ShipList().ShipListView.View).Columns)
            {
                StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal};
                ColorPicker picker = new ColorPicker() { Width = 80, Name = $"GridColumn{item.Header.ToString().Replace(" ", "")}", Foreground = new SolidColorBrush(Colors.Black), FontSize = 12, DropDownBorderThickness = new Thickness(0), ShowDropDownButton = false };
                picker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
                picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources[$"GridColumn{item.Header.ToString().Replace(" ", "")}"]).Color;
                TextBox box = new TextBox() { IsReadOnly = true, Text = $"Highlight Column '{item.Header}'", Background = new SolidColorBrush(Colors.Transparent), Margin = new Thickness(10,0,0,0), BorderThickness = new Thickness(0) };
                box.SetResourceReference(ForegroundProperty, "TextColor");
                box.SetResourceReference(FontSizeProperty, "TextFontSize");
                panel.Children.Add(picker);
                panel.Children.Add(box);

                ShipTreeViewItem.Items.Add(panel);
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
        private void ColorChart_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["ChartColor"]).Color;
        }
        private void ColorSlider_Loaded(object sender, RoutedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            picker.SelectedColor = ((SolidColorBrush)Application.Current.Resources["SliderColor"]).Color;
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

        private void CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            if (WhiteCheckBox == null || RainbowCheckBox == null) return;

            switch (((CheckBox)sender).Content)
            {
                case "White":
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    ((CheckBox)sender).IsChecked = false;
                    SoundPlayer player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "Graphics\\Sounds\\WindowsErrorSound.wav");
                    player.Load();
                    player.Play();

                    break;
                case "Rainbow":
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    if ((bool)((CheckBox)sender).IsChecked) StartRainbow();
                    Application.Current.Resources["TextColor"] = TextColor;
                    Application.Current.Resources["MenuColor"] = MenuColor;
                    Application.Current.Resources["HeadlineColor"] = HeadlineColor;
                    Application.Current.Resources["SliderColor"] = SliderColor;
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
            Application.Current.Resources["ChartColor"] = new SolidColorBrush(ColorFromHSV(counter,1,1));
            Application.Current.Resources["SliderColor"] = new SolidColorBrush(ColorFromHSV(counter,1,1));
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
