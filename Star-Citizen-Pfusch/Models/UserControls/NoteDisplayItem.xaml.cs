using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Star_Citizen_Pfusch.Models.UserControls
{
    /// <summary>
    /// Interaction logic for NoteDisplayItem.xaml
    /// </summary>
    public partial class NoteDisplayItem : UserControl, INotifyPropertyChanged
    {
        private bool _isEditActive;
        private NoteItem noteBuffer;

        public event EventHandler DeleteEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        public NoteItem NoteItem { get; set; }
        public bool IsEditActive
        {
            get
            {
                return !_isEditActive;
            }
            set
            {
                _isEditActive = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsEditActive)));
                }
            }
        }

        public NoteDisplayItem()
        {
            InitializeComponent();
            this.DataContext = this;

            Loaded += NoteDisplayItem_Loaded;
        }

        public NoteDisplayItem(bool isNew)
        {
            InitializeComponent();
            this.DataContext = this;
            EditStackPanel.Visibility = Visibility.Visible;

            Loaded += NoteDisplayItem_Loaded;
        }

        private void NoteDisplayItem_Loaded(object sender, RoutedEventArgs e)
        {
            MdXaml.Markdown mark = new MdXaml.Markdown();

            BodyRichTextbox.Document = mark.Transform(NoteItem.Body);
            BodyTextbox.Text = NoteItem.Body;

            if (NoteItem.Tags == null) return;
            for (int i = 0; i < NoteItem.Tags.Length; i++)
            {
                TextBox textBox = new TextBox()
                {
                    Style = (Style)this.Resources["TagStyle"],
                    Text = NoteItem.Tags[i]
                };
                textBox.MouseDoubleClick += TextBox_MouseDoubleClick1;
                textBox.LostFocus += TextBox_LostFocus;

                TagStackPanel.Children.Insert(TagStackPanel.Children.Count - 1, textBox);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Height = Double.NaN;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (noteBuffer == null || noteBuffer.Tags == null)
            {
                DeleteEvent(this, new EventArgs());
                return;
            }

            IsEditActive = false;
            EditStackPanel.Visibility = Visibility.Hidden;
            BodyRichTextbox.Visibility = Visibility.Visible;
            BodyTextbox.Visibility = Visibility.Hidden;
            NoteItem = noteBuffer;
            BodyTextbox.Text = noteBuffer.Body;
            HeaderTextbox.Text = noteBuffer.Header;
            TagStackPanel.Children.RemoveRange(1, TagStackPanel.Children.Count - 2);
            for (int i = 0; i < noteBuffer.Tags.Length; i++)
            {
                TagStackPanel.Children.Insert(1, new TextBox()
                {
                    Style = (Style)Resources["TagStyle"],
                    Text = noteBuffer.Tags[i]
                });
            }

            PropertyChanged(this, new PropertyChangedEventArgs(nameof(NoteItem)));
        }

        private async void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(new NoteItem()
            {
                Header = NoteItem.Header,
                Body = BodyTextbox.Text,
                LastModified = NoteItem.LastModified,
                Tags = TagStackPanel.Children.OfType<TextBox>().Where(o => o.Style == (Style)Resources["TagStyle"]).Select(o => o.Text).ToArray(),
                id = NoteItem.id
            }), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Config.URL + "/Notes");
            request.Content = content;
            request.Headers.Add("token", Config.SessionToken);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                MdXaml.Markdown mark = new MdXaml.Markdown();

                BodyRichTextbox.Document = mark.Transform(BodyTextbox.Text);
                Debug.WriteLine(BodyRichTextbox.Document.ToString());

                IsEditActive = false;
                BodyRichTextbox.Visibility = Visibility.Visible;
                BodyTextbox.Visibility = Visibility.Hidden;
                EditStackPanel.Visibility = Visibility.Hidden;
            }
        }

        private void TagAddButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = new TextBox()
            {
                Style = (Style)Resources["TagStyle"],
                Text = "Tag"
            };
            textBox.MouseDoubleClick += TextBox_MouseDoubleClick1;
            textBox.LostFocus += TextBox_LostFocus;

            noteBuffer = NoteItem;
            TagStackPanel.Children.Insert(TagStackPanel.Children.Count - 1, textBox);
            EditStackPanel.Visibility = Visibility.Visible;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).IsReadOnly = true;
        }

        private void TextBox_MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {
            ((TextBox)sender).IsReadOnly = false;
            EditStackPanel.Visibility = Visibility.Visible;
            noteBuffer = NoteItem;
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IsEditActive = true;

            BodyRichTextbox.Visibility = Visibility.Hidden;
            BodyTextbox.Visibility = Visibility.Visible;
            EditStackPanel.Visibility = Visibility.Visible;
            noteBuffer = (NoteItem)NoteItem.Clone();
        }

        private Popup popup;
        private void Border_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button button = new Button()
            {
                Style = (Style)Resources["PopupStyle"],
                Content = "Delete"
            };
            button.Click += DeleteButton_Click;

            popup = new Popup()
            {
                Child = button,
                AllowsTransparency = true,
                Placement = PlacementMode.MousePoint,
                StaysOpen = false,
                IsOpen = true
            };

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
            DeleteEvent(this, new EventArgs());
        }
    }
}
