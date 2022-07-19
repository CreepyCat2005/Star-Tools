
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Star_Citizen_Pfusch.Pages.Register
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private static Window currentWindow;
        public Register(Window window)
        {
            currentWindow = window;
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
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (UsernameBox.Text == "" || EMailBox.Text == "" || PasswordBox.Text == "")
            {
                ErrorBox.Text = "Error: Bitte fülle alle Felder aus!";
                return;
            }
            if (!IsValidEmail(EMailBox.Text))
            {
                ErrorBox.Text = "Error: Dies ist keine gültige Email";
                return;
            }

            PasswordHasher hasher = new PasswordHasher();
            string hashed = hasher.hashPassword(PasswordBox.Text);

            using (HttpClient client = new HttpClient())
            {
                AccountItem item = new AccountItem();
                item.Username = UsernameBox.Text;
                item.Password = hashed;
                item.Email = EMailBox.Text;
                item.Salt = hasher.saltString;
                item.AccountData = new AccountDataItem() { Playtime = 0, SessionToken = "" };

                HttpRequestMessage message = new HttpRequestMessage();
                message.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

                HttpResponseMessage ms = await client.PostAsync(Config.URL + "/Register", message.Content);

                if (ms.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    LocalDataManager local = new LocalDataManager();

                    if ((bool)SavePasswordBox.IsChecked) local.SavePassword(UsernameBox.Text,hashed); 
                    currentWindow.Close();
                }
                else
                {
                    var contents = await ms.Content.ReadAsStringAsync();
                    ErrorBox.Text = "Error: " + contents.Substring(contents.IndexOf(" "), contents.IndexOf("\r\n") - contents.IndexOf(" "));
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            currentWindow.Close();
        }


    }
}
