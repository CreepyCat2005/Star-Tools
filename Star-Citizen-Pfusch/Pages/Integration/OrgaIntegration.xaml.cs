using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Pages.Integration
{
    /// <summary>
    /// Interaction logic for OrgaIntegration.xaml
    /// </summary>
    public partial class OrgaIntegration : Page
    {
        private Frame ContentFrame;
        public OrgaIntegration(Frame ContentFrame)
        {
            this.ContentFrame = ContentFrame;

            InitializeComponent();
            init();
        }
        private async void init()
        {

        }
    }
}
