using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Star_Citizen_Pfusch
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Page
    {
        public homeScreen()
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

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ControlTemplate ct = comboBox.Template;
            Popup pop = ct.FindName("PART_Popup", comboBox) as Popup;
            pop.Placement = PlacementMode.Right;
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContentFrame.Content = new Pages.Ships.ShipInformation((ComboBoxItem) comboBox.SelectedItem);
        }
    }
}
