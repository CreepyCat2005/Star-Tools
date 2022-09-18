using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Star_Citizen_Pfusch.Pages
{
    /// <summary>
    /// Interaction logic for startScreen.xaml
    /// </summary>
    public partial class startScreen : Page
    {
        public string Login
        {
            get
            {
                return System.Windows.Application.Current.Resources["start.login"].ToString();
            }
        }
        public string Register
        {
            get
            {
                return System.Windows.Application.Current.Resources["start.register"].ToString();
            }
        }
        public startScreen()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LocalDataManager manager = new LocalDataManager();
            if (manager.passwordExists())
            {
                using (HttpClient client = new HttpClient())
                {
                    AccountItem user = new AccountItem();
                    user.Username = manager.getUsername();
                    user.Password = manager.getPassword();

                    HttpRequestMessage message = new HttpRequestMessage();
                    message.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    HttpResponseMessage ms = await client.PostAsync(Config.URL + "/Login", message.Content);

                    string res = await ms.Content.ReadAsStringAsync();

                    AccountItem item = JsonConvert.DeserializeObject<AccountItem>(res); 

                    if (ms.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Logged in");
                        MainWindow.setContent(new homeScreen());
                        Config.SessionToken = item.AccountData.SessionToken;
                        //login successfull
                        //do sth
                    }
                    else
                    {
                        Debug.WriteLine("login Failed!");
                        //login failed
                        //do sth
                    }
                }
            }
            else
            {
                Window window = new Window();
                window.Title = "Login";
                window.Owner = Application.Current.MainWindow;
                window.Content = new Login.EnterLoginDataPage(window);
                window.Width = 350;
                window.Height = 450;
                window.Show();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            //Window window = new Window();
            //window.Title = "Register";
            //window.Owner = Application.Current.MainWindow;
            //window.Content = new Register.Register(window);
            //window.Width = 350;
            //window.Height = 450;
            //window.Show();
        }

        private void Discord_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = "https://discord.gg/QAR9RTnVVr"
            };
            Process.Start(info);
        }

        private void RSI_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = "https://robertsspaceindustries.com/"
            };
            Process.Start(info);
        }
    }
}
