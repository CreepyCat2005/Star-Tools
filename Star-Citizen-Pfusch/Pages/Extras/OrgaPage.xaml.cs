using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Pages.Extras.OrgaExtras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Pages.Extras
{
    /// <summary>
    /// Interaction logic for OrgaPage.xaml
    /// </summary>
    public partial class OrgaPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isRegistered = false;
        private readonly string link;
        private OrgaItem orgaitem = new OrgaItem();
        private OrgaAdminSettings orgaAdminSettings;
        private OrgaHomePage orgaHomePage = new OrgaHomePage();
        private OrgaMemberPage orgaMemberPage;

        public Visibility IsOwner { get; set; } = Visibility.Collapsed;

        public OrgaPage(string link)
        {
            InitializeComponent();
            ContentDisplay.Navigate(orgaHomePage);
            this.DataContext = this;
            this.link = link;
            Init();
        }

        private async void Init()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://robertsspaceindustries.com" + link);
            request.Headers.Add("Cookie", Config.RSICookieString);
            request.Headers.Add("Host", "robertsspaceindustries.com");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:106.0) Gecko/20100101 Firefox/106.0");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            request.Headers.Add("Accept-Language", "de,en-US;q=0.7,en;q=0.3");
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Sec-Fetch-Dest", "document");
            request.Headers.Add("Sec-Fetch-Mode", "navigate");
            request.Headers.Add("Sec-Fetch-Site", "cross-site");
            HttpResponseMessage response = await client.SendAsync(request);
            string res = await response.Content.ReadAsStringAsync();
            res = Regex.Replace(res, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string[] lineArray = res.Split("\n");

            for (int i = 0; i < lineArray.Length; i++)
            {
                if (lineArray[i].Contains("<div class=\"banner\">") && HeadImage.Source == null)
                {
                    string uri = lineArray[i].Substring(lineArray[i].IndexOf("<img src=\"") + 10, lineArray[i].IndexOf("\" /></div>") - lineArray[i].IndexOf("<img src=\"") - 10);
                    HeadImage.Source = new BitmapImage(new Uri("https://robertsspaceindustries.com" + uri));
                }
                else if (lineArray[i].Contains("<div class=\"logo \">"))
                {
                    string uri = lineArray[i + 1].Substring(lineArray[i + 1].IndexOf("<img src=\"") + 10, lineArray[i + 1].IndexOf("\" />") - lineArray[i + 1].IndexOf("<img src=\"") - 10);
                    IconImage.Source = new BitmapImage(new Uri("https://robertsspaceindustries.com" + uri));
                }
                else if (lineArray[i].Contains(" / <span class=\"symbol\">"))
                {
                    string name = lineArray[i].Substring(lineArray[i].IndexOf("<h1>") + 4, lineArray[i].IndexOf(" / <span class=\"symbol\">") - lineArray[i].IndexOf("<h1>") - 4);
                    string shortName = lineArray[i].Substring(lineArray[i].IndexOf("<span class=\"symbol\">") + 21, lineArray[i].IndexOf("</span></h1>") - lineArray[i].IndexOf("<span class=\"symbol\">") - 21);
                    OrgaNameTextblock.Text = name;
                    orgaitem.ShortName = shortName;
                    orgaitem.Name = name;
                }
                else if (lineArray[i].Contains("<li class=\"primary tooltip-wrap\">"))
                {
                    string uri = lineArray[i + 1].Substring(lineArray[i + 1].IndexOf("<img src=\"") + 10, lineArray[i + 1].IndexOf("\" alt=") - lineArray[i + 1].IndexOf("<img src=\"") - 10);
                    string tooltip = lineArray[i + 1].Substring(lineArray[i + 1].IndexOf("\" alt=\"") + 7, lineArray[i + 1].IndexOf("\" />") - lineArray[i + 1].IndexOf("\" alt=\"") - 7);
                    CategoryStackpanel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri("https://robertsspaceindustries.com" + uri)),
                        Stretch = Stretch.Uniform,
                        ToolTip = tooltip
                    });
                }
                else if (lineArray[i].Contains("<li class=\"secondary tooltip-wrap\">"))
                {
                    string uri = lineArray[i + 1].Substring(lineArray[i + 1].IndexOf("<img src=\"") + 10, lineArray[i + 1].IndexOf("\" alt=") - lineArray[i + 1].IndexOf("<img src=\"") - 10);
                    string tooltip = lineArray[i + 1].Substring(lineArray[i + 1].IndexOf("\" alt=\"") + 7, lineArray[i + 1].IndexOf("\" />") - lineArray[i + 1].IndexOf("\" alt=\"") - 7);
                    CategoryStackpanel.Children.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri("https://robertsspaceindustries.com" + uri)),
                        Stretch = Stretch.Uniform,
                        ToolTip = tooltip
                    });
                }
                else if (lineArray[i].Contains("/admin/overview"))
                {
                    IsOwner = Visibility.Visible;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsOwner)));
                }
            }
            //fetching data from server
            LoadData();
            LoadIcons();
        }
        private void LoadData()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(Config.URL + "/Orga?shortName=" + orgaitem.ShortName).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string res = response.Content.ReadAsStringAsync().Result;
                orgaitem = JsonConvert.DeserializeObject<OrgaItem>(res);
                isRegistered = true;
            }
        }

        private void MemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (orgaMemberPage == null) orgaMemberPage = new OrgaMemberPage(link);
            ContentDisplay.Navigate(orgaMemberPage);
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (orgaAdminSettings == null) orgaAdminSettings = new OrgaAdminSettings(orgaitem, isRegistered);
            ContentDisplay.Navigate(orgaAdminSettings);
        }

        private void LoadIcons()
        {
            CreateIconImage(orgaitem.Ts3URL, "/Graphics/Icons/TS3-Icon.png", new Thickness(12, 2, 0, 2));
            CreateIconImage(orgaitem.DiscordURL, "/Graphics/Icons/Discord-Logo.png", new Thickness(12, 2, 0, 2));
            CreateIconImage(orgaitem.WebsiteURL, "/Graphics/Icons/Website-Icon.png", new Thickness(12, 0, 0, 0));
        }

        private void CreateIconImage(string URL, string iconPath, Thickness margin)
        {
            if (URL != null && !URL.Equals(""))
            {
                BitmapImage bitmap = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + iconPath, UriKind.Absolute));

                Image image = new Image()
                {
                    Source = bitmap,
                    Cursor = Cursors.Hand,
                    Margin = margin,
                    ToolTip = URL
                };

                image.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) => StartProcess(URL);
                LinkStackpanel.Children.Add(image);
            }
        }

        private void StartProcess(string URL)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = URL,
                    UseShellExecute = true
                });
            }
            catch { }
        }

        private void MainPageButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDisplay.Navigate(orgaHomePage);
        }

        private void ShipButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
