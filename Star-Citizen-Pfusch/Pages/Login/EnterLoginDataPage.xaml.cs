using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace Star_Citizen_Pfusch.Pages.Login
{
    /// <summary>
    /// Interaction logic for EnterLoginDataPage.xaml
    /// </summary>
    public partial class EnterLoginDataPage : Page
    {
        private static Window currentWindow;
        public EnterLoginDataPage(Window window)
        {
            currentWindow = window;
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            currentWindow.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameBox.Text == "" || PasswordBox.Text == "")
            {
                ErrorBox.Text = "Error: Bitte fülle alle Felder aus!";
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Config.URL + "/Login?username=" + UsernameBox.Text);
                string salt = await response.Content.ReadAsStringAsync();

                PasswordHasher hasher = new PasswordHasher();
                string hashed = hasher.hashPassword(PasswordBox.Text, salt);
                LoginUser user = new LoginUser();
                user.Username = UsernameBox.Text;
                user.Password = hashed;

                HttpRequestMessage message = new HttpRequestMessage();
                message.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                HttpResponseMessage ms = await client.PostAsync(Config.URL + "/Login", message.Content);

                if (ms.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    currentWindow.Close();
                    MainWindow.setContent(new homeScreen());
                }
                else
                {
                    var contents = await ms.Content.ReadAsStringAsync();
                    ErrorBox.Text = "Error: " + contents.Substring(contents.IndexOf(" "), contents.IndexOf("\r\n") - contents.IndexOf(" "));
                }
            }
        }
    }
}
