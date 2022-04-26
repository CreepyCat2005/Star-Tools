using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public startScreen()
        {
            ResourceDictionary dictionary = new ResourceDictionary();

            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dictionary.Source = new Uri("/languages/english.xaml", UriKind.Relative);
                    break;
                case "de-AT":
                    dictionary.Source = new Uri("/languages/german.xaml", UriKind.Relative);
                    break;
                case "de-DE":
                    dictionary.Source = new Uri("/languages/german.xaml", UriKind.Relative);
                    break;

            }
            this.Resources.MergedDictionaries.Add(dictionary);

            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LocalDataManager manager = new LocalDataManager();
            if (manager.passwordExists())
            {
                using (HttpClient client = new HttpClient())
                {
                    LoginUser user = new LoginUser();
                    user.Username = manager.getUsername();
                    user.Password = manager.getPassword();

                    HttpRequestMessage message = new HttpRequestMessage();
                    message.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    HttpResponseMessage ms = await client.PostAsync(Config.URL + "/Login", message.Content);

                    if (ms.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Logged in");
                        MainWindow.setContent(new homeScreen());
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

            Window window = new Window();
            window.Title = "Register";
            window.Owner = Application.Current.MainWindow;
            window.Content = new Register.Register(window);
            window.Width = 350;
            window.Height = 450;
            window.Show();
        }
    }
}
