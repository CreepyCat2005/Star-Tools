using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Pages.Home.Widgets
{
    /// <summary>
    /// Interaction logic for PlaytimeHistory.xaml
    /// </summary>
    public partial class PlaytimeHistory : UserControl
    {
        public PlaytimeHistory()
        {
            InitializeComponent();
            init();
        }

        private async void init()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.URL + "/AccountData?History=true");
            request.Headers.Add("Token", Config.SessionToken);
            HttpResponseMessage response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();

            AccountDataItem item = JsonConvert.DeserializeObject<AccountDataItem>(res);

            PlaytimeTextBox.Text = formatePlayTime((int)item.Playtime + item.PlaytimeHistory.Sum());
            MeasuredTextBox.Text = ((DateTime)item.AccountCreatedOn).ToShortDateString();
        }
        
        private string formatePlayTime(int playtime)
        {
            int hour = playtime / 60;
            int minute = playtime - (hour * 60);
            return $"{hour}h {minute}m";
        }
    }
}
