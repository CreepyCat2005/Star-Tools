using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Star_Citizen_Pfusch.Pages.Extras.OrgaExtras
{
    /// <summary>
    /// Interaction logic for OrgaAdminSettings.xaml
    /// </summary>
    public partial class OrgaAdminSettings : Page, INotifyPropertyChanged
    {
        private OrgaItem restoreItem;
        private HttpClient client = new HttpClient();

        private OrgaItem orgaItem;
        public OrgaItem OrgaItem
        {
            get
            {
                return orgaItem;
            }
            set
            {
                orgaItem = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(OrgaItem)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OrgaAdminSettings(OrgaItem item, bool IsRegistered)
        {
            orgaItem = item;
            restoreItem = (OrgaItem)item.Clone();

            InitializeComponent();
            this.DataContext = this;

            if (IsRegistered)
            {
                OrgaAddCheckBox.Visibility = Visibility.Collapsed;
                SettingsBox.Visibility = Visibility.Visible;
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (SendOrgaToServer())
            {
                ((CheckBox)sender).Visibility = Visibility.Collapsed;
                SettingsBox.Visibility = Visibility.Visible;
            }
        }

        private bool SendOrgaToServer()
        {
            OrgaItem.RegisteredAt = DateTime.UtcNow;

            HttpResponseMessage response = client.PostAsync(Config.URL + "/Orga",new StringContent(JsonConvert.SerializeObject(OrgaItem),Encoding.UTF8,"application/json")).Result;
            string res = response.Content.ReadAsStringAsync().Result;

            if (res.Equals("true")) return true;
            else return false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckURL(OrgaItem.WebsiteURL) ||
                !CheckURL(OrgaItem.DiscordURL))
            {
                Debug.WriteLine("Invalid Url!");
                return;
            }

            HttpResponseMessage response = client.PutAsync(Config.URL + "/Orga", new StringContent(JsonConvert.SerializeObject(OrgaItem), Encoding.UTF8, "application/json")).Result;
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                restoreItem = (OrgaItem)OrgaItem.Clone();
                Debug.WriteLine("I am a cat!");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OrgaItem = (OrgaItem)restoreItem.Clone();
        }

        private bool CheckURL(string url)
        {
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
