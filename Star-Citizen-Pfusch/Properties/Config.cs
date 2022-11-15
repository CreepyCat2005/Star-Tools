using Newtonsoft.Json;
using Star_Citizen_Pfusch.Functions;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch
{
    class Config
    {
        private static string _browserType;

        //public static string URL = @"http://45.88.109.120:80";
        public static string URL = @"http://localhost:5001";
        public static string RSICookieString;
        public static string SessionToken;
        public static bool ModernShipList;
        public static int ChartResolution = 12;
        public static string BrowserType
        {
            get
            {
                return _browserType;
            }
            set
            {
                _browserType = value;
                UserConfigItem userConfig = JsonConvert.DeserializeObject<UserConfigItem>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Config/UserConfig.cfg"));
                userConfig.BrowserType = _browserType;
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Config/UserConfig.cfg", JsonConvert.SerializeObject(userConfig));
            }
        }
    }
}
