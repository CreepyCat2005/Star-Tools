using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Pages.Integration
{
    /// <summary>
    /// Interaction logic for FleetyardIntegration.xaml
    /// </summary>
    public partial class FleetyardIntegration : Page
    {
        private Frame ContentFrame;
        public FleetyardIntegration(Frame ContentFrame)
        {
            this.ContentFrame = ContentFrame;

            InitializeComponent();
            init();
        }
        private async void init()
        {

            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.fleetyards.net/v1/fleets/current");
            requestMessage.Headers.Add("Cookie", $"FLTYRD={LocalDataManager.GetFleetyardToken()}");
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            string res = await response.Content.ReadAsStringAsync();

            JArray jArray = JArray.Parse(res);

            foreach (var json in jArray)
            {
                FleetYardOrgaItem item = JsonConvert.DeserializeObject<FleetYardOrgaItem>(json.ToString());

                ModuleList.Items.Add(new ListBoxItem() { Content = new FleetyardOrgaContainer(item, ContentFrame), Width = 200, Height = 200 });

            }
        }
    }
}
