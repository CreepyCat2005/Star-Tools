using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
    /// Interaction logic for LicenseSettings.xaml
    /// </summary>
    public partial class LicenseSettings : Page
    {
        public LicenseSettings()
        {
            ResourceDictionary dictionary = new ResourceDictionary();

            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dictionary.Source = new Uri("/languages/english.xaml", UriKind.Relative);
                    break;
                case "de-AT":
                    dictionary.Source = new Uri("/languages/german.xaml", UriKind.Relative);
                    break;
                case "de-DE":
                    dictionary.Source = new Uri("/languages/german.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dictionary);

            InitializeComponent();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Content = new StringContent(JsonConvert.SerializeObject(new Models.ProductKeyModel() { Email = Config.email, key = licenseKeyBox.Text }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(Config.URL + "/ProductKey", request.Content);
                string res = await response.Content.ReadAsStringAsync();

                Models.ProductKeyModel key = JsonConvert.DeserializeObject<Models.ProductKeyModel>(res);

                if (response.StatusCode == System.Net.HttpStatusCode.OK && key.valid)
                {
                    validLabel.Foreground = new SolidColorBrush() { Color = Colors.Green };
                    validLabel.Content = FindResource("ProductKey.keyActive");
                }
                else
                {
                    validLabel.Foreground = new SolidColorBrush() { Color = Colors.Red };
                    validLabel.Content = FindResource("ProductKey.keyNotActive");
                }
            }
        }
    }
}
