using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        private GeneralSettings GeneralSettings = new GeneralSettings();
        private AccountSettings AccountSettings;
        private PrivacySettings PrivacySettings;
        public SettingsMenu()
        {
            InitializeComponent();
            this.Unloaded += SettingsMenu_Unloaded;
        }

        private void SettingsMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            UserConfigItem userConfig = new UserConfigItem()
            {
                HeadlineColor = ((SolidColorBrush)Application.Current.Resources["HeadlineColor"]).Color.ToString(),
                MenuColor = ((SolidColorBrush)Application.Current.Resources["MenuColor"]).Color.ToString(),
                TextColor = ((SolidColorBrush)Application.Current.Resources["TextColor"]).Color.ToString(),
                Theme = (string)Application.Current.Resources["Theme"],
                TextFontSize = (double)Application.Current.Resources["TextFontSize"],
                MenuFontSize = (double)Application.Current.Resources["MenuFontSize"],
                HeadlineFontSize = (double)Application.Current.Resources["HeadlineFontSize"],
                IsRainbowActive = (bool)Application.Current.Resources["RainbowModeActive"],
                RainbowValue = (double)Application.Current.Resources["RainbowModeValue"],
                GridColumnName = ((SolidColorBrush)Application.Current.Resources["GridColumnName"]).Color.ToString(),
                GridColumnAfterburner = ((SolidColorBrush)Application.Current.Resources["GridColumnAfterburner"]).Color.ToString(),
                GridColumnCareer = ((SolidColorBrush)Application.Current.Resources["GridColumnCareer"]).Color.ToString(),
                GridColumnCargo = ((SolidColorBrush)Application.Current.Resources["GridColumnCargo"]).Color.ToString(),
                GridColumnHealth = ((SolidColorBrush)Application.Current.Resources["GridColumnHealth"]).Color.ToString(),
                GridColumnHydrogenFuel = ((SolidColorBrush)Application.Current.Resources["GridColumnHydrogenFuel"]).Color.ToString(),
                GridColumnManufacturer = ((SolidColorBrush)Application.Current.Resources["GridColumnManufacturer"]).Color.ToString(),
                GridColumnMass = ((SolidColorBrush)Application.Current.Resources["GridColumnMass"]).Color.ToString(),
                GridColumnPitch = ((SolidColorBrush)Application.Current.Resources["GridColumnPitch"]).Color.ToString(),
                GridColumnPrice = ((SolidColorBrush)Application.Current.Resources["GridColumnPrice"]).Color.ToString(),
                GridColumnRole = ((SolidColorBrush)Application.Current.Resources["GridColumnRole"]).Color.ToString(),
                GridColumnQuantumFuel = ((SolidColorBrush)Application.Current.Resources["GridColumnQuantumFuel"]).Color.ToString(),
                GridColumnRoll = ((SolidColorBrush)Application.Current.Resources["GridColumnRoll"]).Color.ToString(),
                GridColumnShieldType = ((SolidColorBrush)Application.Current.Resources["GridColumnShieldType"]).Color.ToString(),
                GridColumnSize = ((SolidColorBrush)Application.Current.Resources["GridColumnSize"]).Color.ToString(),
                GridColumnSpeed = ((SolidColorBrush)Application.Current.Resources["GridColumnSpeed"]).Color.ToString(),
                GridColumnYaw = ((SolidColorBrush)Application.Current.Resources["GridColumnYaw"]).Color.ToString(),
                BackgroundColor = ((SolidColorBrush)Application.Current.Resources["BackgroundColor"]).Color.ToString(),
                DarkBackgroundColor = ((SolidColorBrush)Application.Current.Resources["DarkBackgroundColor"]).Color.ToString(),
                ChartColor = ((SolidColorBrush)Application.Current.Resources["ChartColor"]).Color.ToString(),
                ChartPointColor = ((SolidColorBrush)Application.Current.Resources["ChartPointColor"]).Color.ToString(),
                SliderColor = ((SolidColorBrush)Application.Current.Resources["SliderColor"]).Color.ToString(),
                DefaultStartSize = (string)Application.Current.Resources["DefaultStartSize"],
                IsModernShipListActive = Config.ModernShipList,
                ChartResolution = Config.ChartResolution
            };
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Config/UserConfig.cfg", JsonConvert.SerializeObject(userConfig));
        }

        private void GeneralItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            contentDisplay.Navigate(GeneralSettings);
        }
        private void LicenseItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            contentDisplay.Content = new LicenseSettings();
        }
        private void dataItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PrivacySettings == null) PrivacySettings = new PrivacySettings();
            contentDisplay.Navigate(PrivacySettings);
        }
        private void AppearanceItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            contentDisplay.Navigate(AppearanceSettings);
        }

        private void AccountSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AccountSettings == null) AccountSettings = new AccountSettings();
            contentDisplay.Navigate(AccountSettings);
        }
    }
}
