using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for SelectBrowserPopup.xaml
    /// </summary>
    public partial class SelectBrowserPopup : UserControl
    {
        private string SelectedBrowser;
        private Popup Popup;
        public SelectBrowserPopup(Popup popup)
        {
            Popup = popup;

            InitializeComponent();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in MasterStackPanel.Children.OfType<CheckBox>())
            {
                item.IsChecked = false;
            }

            ((CheckBox)sender).IsChecked = true;
            SelectedBrowser = ((CheckBox)sender).Content.ToString();
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            Config.BrowserType = SelectedBrowser;
            Popup.IsOpen = false;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = false;
        }
    }
}
