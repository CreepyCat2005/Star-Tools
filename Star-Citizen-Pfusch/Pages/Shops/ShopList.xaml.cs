using Newtonsoft.Json;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Pages.SettingsFolder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Star_Citizen_Pfusch.Pages.Shops
{
    /// <summary>
    /// Interaction logic for ShopList.xaml
    /// </summary>
    public partial class ShopList : Page
    {
        private ObservableCollection<TreeViewItem> treeViewItems = new ObservableCollection<TreeViewItem>();

        public ShopList()
        {
            InitializeComponent();
            init();
        }

        private async void init()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Config.URL + "/Shop");
            string res = await response.Content.ReadAsStringAsync();

            List<ShopItem> shopItems = JsonConvert.DeserializeObject<List<ShopItem>>(res);

            foreach (var item in shopItems)
            {
                TreeViewItem treeViewItem = new TreeViewItem() { Header = item.name + " | " + item.location };
                foreach (var subItem in item.inventory)
                {
                    TreeViewItem viewItem = new TreeViewItem() { Header = subItem.name + " | " + String.Format("{0:n0} aUEC", subItem.price), FontSize = 24, Margin = new Thickness(0,2,0,2) };
                    viewItem.SetResourceReference(ForegroundProperty, "TextColor");
                    treeViewItem.Items.Add(viewItem);
                }
                treeViewItems.Add(treeViewItem);
            }
            TreeView.ItemsSource = treeViewItems;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            string boxText = box.Text;
            if (box.Text.Equals("Search")) boxText = "";
            for (int i = 0; i < treeViewItems.Count; i++)
            {
                bool hit = false;
                foreach (TreeViewItem item in treeViewItems[i].Items)
                {
                    if (item.Header.ToString().ToLower().Contains(boxText.ToLower()) || treeViewItems[i].Header.ToString().ToLower().Contains(boxText.ToLower()))
                    {
                        item.Visibility = Visibility.Visible;
                        hit = true;
                    }
                    else
                    {
                        item.Visibility = Visibility.Collapsed;
                    }
                }
                if (!hit)
                {
                    treeViewItems[i].Visibility = Visibility.Collapsed;
                }
                else
                {
                    treeViewItems[i].Visibility = Visibility.Visible;
                }
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Text.Equals("Search"))
            {
                box.Text = "";
                box.Foreground = (SolidColorBrush)Application.Current.Resources["TextColor"];

            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Text.Equals(""))
            {
                box.Text = "Search";
                box.Foreground = new SolidColorBrush(Color.FromArgb(255, 80, 80, 80));

                
            }
        }
    }
}
