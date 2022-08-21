using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
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
using System.Windows.Forms;
using System.Windows.Media;

namespace Star_Citizen_Pfusch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        private static MainWindow window;
        private NotifyIcon notifyIcon = new NotifyIcon();

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
                this.DataContext = this;

                loadConfig();
                InitializeComponent();

                Width = double.Parse(((string)System.Windows.Application.Current.Resources["DefaultStartSize"]).Split("x")[0]);
                Height = double.Parse(((string)System.Windows.Application.Current.Resources["DefaultStartSize"]).Split("x")[1]);

                if (Width == Screen.PrimaryScreen.Bounds.Width && Height == Screen.PrimaryScreen.Bounds.Height) this.WindowState = WindowState.Maximized;

                StartServer();
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
        private void loadConfig()
        {
            if (!File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Config/UserConfig.cfg").Equals(""))
            {
                UserConfigItem userConfig = JsonConvert.DeserializeObject<UserConfigItem>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Config/UserConfig.cfg"));
                var colors = userConfig.GetType().GetProperties().Where(o => o.PropertyType == typeof(string)).ToList();
                var fonts = userConfig.GetType().GetProperties().Where(o => o.PropertyType == typeof(double)).ToList();

                foreach (var color in colors)
                {
                    if (color.Name.Equals("Theme") || color.Name.Equals("DefaultStartSize") || color.GetValue(userConfig) == null) continue;
                    System.Windows.Application.Current.Resources[color.Name] = new BrushConverter().ConvertFrom(color.GetValue(userConfig));
                }
                System.Windows.Application.Current.Resources["Theme"] = userConfig.Theme;
                if (userConfig.DefaultStartSize != null) System.Windows.Application.Current.Resources["DefaultStartSize"] = userConfig.DefaultStartSize;

                foreach (var font in fonts)
                {
                    if (font.Name.Equals("RainbowValue") || font.GetValue(userConfig) == null) continue;
                    System.Windows.Application.Current.Resources[font.Name] = font.GetValue(userConfig);
                }
            }
        }
        private void Menu_Open(object sender, EventArgs e)
        {
            window.Show();
            window.WindowState = WindowState.Normal;
        }
        private async void Menu_Close(object sender, EventArgs e)
        {
            await PlaytimeCounter.savePlaytime();
            notifyIcon.Dispose();
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
