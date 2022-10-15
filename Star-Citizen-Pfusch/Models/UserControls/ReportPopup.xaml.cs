using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for ReportPopup.xaml
    /// </summary>
    public partial class ReportPopup : UserControl
    {
        private Popup Popup;
        private string PageName;
        public ReportPopup(Popup popup, string pageName)
        {
            this.PageName = pageName;
            this.Popup = popup;
            InitializeComponent();
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!MessageBox.Text.Equals(""))
            {
                ReportItem reportItem = new ReportItem()
                {
                    SessionToken = Config.SessionToken,
                    PageName = PageName,
                    ReportMessage = MessageBox.Text,
                    Time = DateTime.Now
                };

                HttpClient client = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(reportItem), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(Config.URL + "/Report", content);
                Debug.WriteLine(await response.Content.ReadAsStringAsync());

                Popup.IsOpen = false;
            }
            else
            {
                MessageBox.Text = "Message darf nicht leer sein!";
                MessageBox.Foreground = new SolidColorBrush(Colors.Red);

                await Task.Delay(2000);

                MessageBox.Text = "";
                MessageBox.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = false;
        }
    }
}
