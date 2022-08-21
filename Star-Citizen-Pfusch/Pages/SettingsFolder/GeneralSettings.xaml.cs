using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace Star_Citizen_Pfusch.Pages.SettingsFolder
{
    /// <summary>
    /// Interaction logic for GeneralSettings.xaml
    /// </summary>
    public partial class GeneralSettings : Page
    {
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
                    if (item.Equals(Application.Current.Resources["DefaultStartSize"])) ResolutionBox.Items.Add(new ComboBoxItem() { Content = item, IsSelected = true});
                    else ResolutionBox.Items.Add(new ComboBoxItem() { Content = item });
                }
            }

            if (Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run").GetValue("Star-Tools") != null) AutostartCheckBox.IsChecked = true;
            else AutostartCheckBox.IsChecked = false;
        }

        private void AutoStartCheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            if ((bool)((CheckBox)sender).IsChecked)
            {
                Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run").SetValue("Star-Tools",AppDomain.CurrentDomain.BaseDirectory + "Star-Citizen-Pfusch.exe");
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
    }
}
