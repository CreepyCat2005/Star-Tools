using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.UserControls;

namespace Star_Citizen_Pfusch.Pages.Extras
{
    /// <summary>
    /// Interaction logic for NotesPage.xaml
    /// </summary>
    public partial class NotesPage : Page
    {
        public ObservableCollection<NoteDisplayItem> NoteDisplayItems = new ObservableCollection<NoteDisplayItem>();
        public NotesPage()
        {
            InitializeComponent();

            Loaded += NotesPage_Loaded;
        }

        private void NotesPage_Loaded(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Config.URL + "/Notes");
            request.Headers.Add("token", Config.SessionToken);
            HttpResponseMessage response = client.SendAsync(request).Result;
            string res = response.Content.ReadAsStringAsync().Result;

            JArray jArray = JArray.Parse(res);
            NoteDisplayItems.Clear();

            foreach (var item in jArray)
            {
                NoteItem noteItem = JsonConvert.DeserializeObject<NoteItem>(item.ToString());

                NoteDisplayItem noteDisplayItem = new NoteDisplayItem()
                {
                    NoteItem = noteItem
                };
                noteDisplayItem.DeleteEvent += Item_DeleteEvent;

                NoteDisplayItems.Add(noteDisplayItem);
            }

            NoteDisplayList.ItemsSource = NoteDisplayItems;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            NoteItem note = new NoteItem()
            {
                id = Guid.NewGuid().ToString(),
                Body = "Empty",
                Header = "New Note",
                LastModified = DateTime.Now.ToString("dd.MM.yyyy"),
            };
            NoteDisplayItem item = new NoteDisplayItem(true)
            {
                NoteItem = note,
                IsEditActive = true
            };
            item.DeleteEvent += Item_DeleteEvent;

            NoteDisplayItems.Add(item);
        }

        private async void Item_DeleteEvent(object sender, EventArgs e)
        {
            NoteDisplayItems.Remove((NoteDisplayItem)sender);

            StringContent content = new StringContent(JsonConvert.SerializeObject(((NoteDisplayItem)sender).NoteItem), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Config.URL + "/Notes");
            request.Content = content;
            request.Headers.Add("token", Config.SessionToken);
            await client.SendAsync(request);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            string boxText = box.Text;
            if (box.Text.Equals("Search")) boxText = "";
            for (int i = 0; i < NoteDisplayItems.Count; i++)
            {
                if (NoteDisplayItems[i].NoteItem.Header.ToLower().Contains(boxText.ToLower()))
                {
                    NoteDisplayItems[i].Visibility = Visibility.Visible;
                }
                else
                {
                    NoteDisplayItems[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Text.Equals("Search"))
            {
                box.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Text.Equals(""))
            {
                box.Text = "Search";
            }
        }
    }
}
