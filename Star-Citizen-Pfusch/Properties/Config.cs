using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Star_Citizen_Pfusch
{
    class Config
    {

        public static string URL = @"http://45.88.109.120:80";
        //public static string URL = @"http://localhost:5001";
        public static string RSICookieString;
        public static string SessionToken;
        public static bool ModernShipList;
        public static int ChartResolution = 12;
        public static bool SendPledgeData;
        public static BrowserEnum BrowserType;
    }
}
