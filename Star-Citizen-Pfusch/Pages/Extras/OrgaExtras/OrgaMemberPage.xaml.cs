using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Star_Citizen_Pfusch.Pages.Extras.OrgaExtras
{
    /// <summary>
    /// Interaction logic for OrgaMemberPage.xaml
    /// </summary>
    public partial class OrgaMemberPage : Page
    {
        private List<OrgaMemberItem> members = new List<OrgaMemberItem>();

        public OrgaMemberPage(string link)
        {
            InitializeComponent();

            Init(link);
        }

        private async void Init(string link)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://robertsspaceindustries.com{link}/members");
            string res = await response.Content.ReadAsStringAsync();
            res = Regex.Replace(res, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string[] lineArray = res.Split("\n");

            for (int i = 0; i < lineArray.Length; i++)
            {
                if (lineArray[i].Contains("<span class=\"thumb\">"))
                {
                    members.Add(new OrgaMemberItem()
                    {
                        ImageURI = new Uri("https://robertsspaceindustries.com" + lineArray[i + 1].Substring(lineArray[i + 1].IndexOf("<img src=\"") + 10, lineArray[i + 1].IndexOf("\" />") - lineArray[i + 1].IndexOf("<img src=\"") - 10), UriKind.Absolute)
                    });
                }
                else if (lineArray[i].Contains("<span class=\"trans-03s name"))
                {
                    members.Last().Name = lineArray[i].Substring(lineArray[i].IndexOf(">") + 1, lineArray[i].IndexOf("</span>") - lineArray[i].IndexOf(">") - 1);
                    members.Last().Nickname = lineArray[i + 1].Substring(lineArray[i + 1].IndexOf(">") + 1, lineArray[i + 1].IndexOf("</span>") - lineArray[i + 1].IndexOf(">") - 1);
                    members.Last().Rank = lineArray[i + 8].Substring(lineArray[i + 8].IndexOf(">") + 1, lineArray[i + 8].IndexOf("</span>") - lineArray[i + 8].IndexOf(">") - 1);
                    i += 20;
                }
            }
        }
    }
}
