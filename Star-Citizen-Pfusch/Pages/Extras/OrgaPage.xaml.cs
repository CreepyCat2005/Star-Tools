using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Star_Citizen_Pfusch.Pages.Extras
{
    /// <summary>
    /// Interaction logic for OrgaPage.xaml
    /// </summary>
    public partial class OrgaPage : Page
    {

        public OrgaPage(string link)
        {
            InitializeComponent();
            this.DataContext = this;

            init(link);
        }

        private async void init(string link)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://robertsspaceindustries.com" + link);
            string res = await response.Content.ReadAsStringAsync();
            res = Regex.Replace(res, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string[] lineArray = res.Split("\n");

            for (int i = 0; i < lineArray.Length; i++)
            {
                if (lineArray[i].Contains("<img src=\"/rsi/static/images/organization/defaults") && HeadImage.Source == null)
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
                    OrgaNameTextblock.Text = name;
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
            }



        }
    }
}
