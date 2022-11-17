using Microsoft.Win32;
using Star_Citizen_Pfusch.Models.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Star_Citizen_Pfusch.Pages.SettingsFolder
{
    /// <summary>
    /// Interaction logic for GeneralSettings.xaml
    /// </summary>
    public partial class GeneralSettings : Page
    {
        private string[] BrowserNames = { "Firefox", "Chrome", "Opera", "OperaGX", "Edge" };

        public GeneralSettings()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            string[] resolutions = { "640x360", "854x480", "1280x720", "1920x1080", "2560x1440", "3840x2160" };

            foreach (var item in resolutions)
            {
                if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width >= int.Parse(item.Split("x")[0]) && System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height >= int.Parse(item.Split("x")[1]))
                {
                    if (item.Equals(Application.Current.Resources["DefaultStartSize"])) ResolutionBox.Items.Add(new ComboBoxItem() { Content = item, IsSelected = true });
                    else ResolutionBox.Items.Add(new ComboBoxItem() { Content = item });
                }
            }

            if (Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run").GetValue("Star-Tools") != null) AutostartCheckBox.IsChecked = true;
            else AutostartCheckBox.IsChecked = false;

            foreach (var item in BrowserNames)
            {
                if (item.Equals(Config.BrowserType.ToString())) BrowserComboBox.Items.Add(new ComboBoxItem() 
                { 
                    Content = item,
                    IsSelected = true
                });
                else BrowserComboBox.Items.Add(new ComboBoxItem()
                {
                    Content = item
                });
            }
        }

        private void AutoStartCheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            if ((bool)((CheckBox)sender).IsChecked)
            {
                Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run").SetValue("Star-Tools", AppDomain.CurrentDomain.BaseDirectory + "Star-Citizen-Pfusch.exe");
            }
            else
            {
                Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run").DeleteValue("Star-Tools");
            }
        }

        private void ResolutionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.Resources["DefaultStartSize"] = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content;
        }

        private void BrowserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Config.BrowserType = Enum.Parse<BrowserEnum>(((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString());
        }
    }
}
