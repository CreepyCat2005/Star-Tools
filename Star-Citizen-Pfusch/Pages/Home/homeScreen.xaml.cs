using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void MainMenuButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button bu = (Button)sender;
            bu.Foreground = Brushes.Pink;
        }
    }
}
