using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : Page
    {
        private AppearanceSettings AppearanceSettings = new AppearanceSettings();
        public SettingsMenu()
        {
            InitializeComponent();
        }

        private void GeneralItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void LicenseItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            contentDisplay.Content = new LicenseSettings();
        }

        private void dataItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void AppearanceItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            contentDisplay.Navigate(AppearanceSettings);
        }
    }
}
