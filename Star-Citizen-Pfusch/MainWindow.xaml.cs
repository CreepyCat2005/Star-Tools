using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Pages;
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

namespace Star_Citizen_Pfusch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static MainWindow window;

        public MainWindow()
        {
            window = this;

            InitializeComponent();

            this.Content = new startScreen();
        }

        private static MainWindow getWindow()
        {
            return window;
        }
        public static void setContent(object sender)
        {
            getWindow().Content = sender;
        }
    }
}
