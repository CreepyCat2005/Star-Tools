using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.ComponentModel;
using System.Net.Http;
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

        public OrgaAdminSettings(OrgaItem item)
        {
            orgaItem = item;
            InitializeComponent();
            this.DataContext = this;

            Init();
        }

        private async void Init()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Config.URL + "/Orga?shortName=" + OrgaItem.ShortName);
            string res = await response.Content.ReadAsStringAsync();

            OrgaItem = JsonConvert.DeserializeObject<OrgaItem>(res);
            OrgaAddCheckBox.Visibility = Visibility.Collapsed;
            SettingsBox.Visibility = Visibility.Visible;
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

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PostAsync(Config.URL + "/Orga",new StringContent(JsonConvert.SerializeObject(OrgaItem),Encoding.UTF8,"application/json")).Result;
            string res = response.Content.ReadAsStringAsync().Result;

            if (res.Equals("true")) return true;
            else return false;
        }
    }
}
