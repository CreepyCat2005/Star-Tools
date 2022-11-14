using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Pages.Extras.OrgaExtras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
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

        public Visibility IsOwner { get; set; } = Visibility.Collapsed;

        public OrgaPage(string link)
        {
            InitializeComponent();
            this.DataContext = this;
            this.link = link;
            Init();
        }

        private async void Init()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://robertsspaceindustries.com" + link);
            request.Headers.Add("Cookie", LocalDataManager.GetRSICookieString());
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
                if (lineArray[i].Contains("<img src=\"/rsi/static/images/organization/defaults") && HeadImage.Source == null)
                {
                    string uri = lineArray[i].Substring(lineArray[i].IndexOf("<div class=\"banner\"><img src=\"") + 30, lineArray[i].IndexOf("\" /></div>") - lineArray[i].IndexOf("<div class=\"banner\"><img src=\"") - 30);
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

        private async void MemberButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://robertsspaceindustries.com" + link + "/members");
            string res = await response.Content.ReadAsStringAsync();
            res = Regex.Replace(res, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string[] lineArray = res.Split("\n");
            Dictionary<int, string> dict = new Dictionary<int, string>();

            for (int i = 0; i < lineArray.Length; i++)
            {
                if (lineArray[i].Contains("<input type=\"radio\" class=\"js-rank js-form-data\" name=\"rank\""))
                {
                    dict.Add(int.Parse(lineArray[i].Substring(lineArray[i].IndexOf("value=\"") + 7, lineArray[i].IndexOf("\" >") - lineArray[i].IndexOf("value=\"") - 7)),
                        lineArray[i + 1].Substring(lineArray[i + 1].IndexOf("<span>") + 6, lineArray[i + 1].IndexOf("</span>") - lineArray[i + 1].IndexOf("<span>") - 6));
                }
            }
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (orgaAdminSettings == null) orgaAdminSettings = new OrgaAdminSettings(orgaitem, isRegistered);
            ContentDisplay.Navigate(orgaAdminSettings);
        }

        private void LoadIcons()
        {
            if (orgaitem.Ts3URL != null)
            {
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri("/Graphics/Icons/Website-Icon.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(8,0,0,0)
                };
                image.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) => Process.Start(new ProcessStartInfo
                {
                    FileName = orgaitem.Ts3URL,
                    UseShellExecute = true
                });
                LinkStackpanel.Children.Add(image);
            }

            if (orgaitem.DiscordURL != null)
            {
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri("/Graphics/Icons/Discord-Logo.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(8, 2, 0, 2)
                };
                image.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) => Process.Start(new ProcessStartInfo
                {
                    FileName = orgaitem.DiscordURL,
                    UseShellExecute = true
                });
                LinkStackpanel.Children.Add(image);
            }

            if (orgaitem.WebsiteURL != null)
            {
                Image image = new Image()
                {
                    Source = new BitmapImage(new Uri("/Graphics/Icons/Website-Icon.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                    Margin = new Thickness(8, 0, 0, 0)
                };
                image.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) => Process.Start(new ProcessStartInfo
                {
                    FileName = orgaitem.WebsiteURL,
                    UseShellExecute = true
                });
                LinkStackpanel.Children.Add(image);
            }
        }
    }
}
