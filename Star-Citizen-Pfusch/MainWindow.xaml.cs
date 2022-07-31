using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
            Process[] processes = Process.GetProcessesByName("Star-Citizen-Pfusch");
            if(processes.Length > 1)
            {
                var client = new NamedPipeClientStream("Star-Tools-Pipe");
                client.Connect();
                client.Close();
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                window = this;

                InitializeComponent();
                StartServer();
                NotifyIcon notifyIcon = new NotifyIcon();
                notifyIcon.Icon = new System.Drawing.Icon(Directory.GetCurrentDirectory() + "/Star-Tools-Logo.ico");
                notifyIcon.Visible = true;
                notifyIcon.ContextMenuStrip = new ContextMenuStrip();
                notifyIcon.ContextMenuStrip.Items.Add("Open", null, Menu_Open);
                notifyIcon.ContextMenuStrip.Items.Add("Close", null, Menu_Close);

                this.Content = new startScreen();
            }
        }
        private void StartServer()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var server = new NamedPipeServerStream("Star-Tools-Pipe");
                    server.WaitForConnection();
                    server.ReadByte();

                    Debug.WriteLine("HIASA");
                    window.Dispatcher.Invoke(() =>
                    {
                        window.Show();
                        window.WindowState = WindowState.Normal;
                    });
                    server.Close();
                }
            });
        }
        private void Menu_Open(object sender, EventArgs e)
        {
            window.Show();
            window.WindowState = WindowState.Normal;
        }
        private async void Menu_Close(object sender, EventArgs e)
        {
            await PlaytimeCounter.savePlaytime();
            System.Windows.Application.Current.Shutdown();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (window != null)
            {
                e.Cancel = true;

                window.Hide();

                base.OnClosing(e);
            }
        }
        public static MainWindow getWindow()
        {
            return window;
        }
        public static void setContent(object sender)
        {
            getWindow().Content = sender;
        }
    }
}
