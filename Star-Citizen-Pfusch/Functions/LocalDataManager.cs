using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Functions
{
    class LocalDataManager
    {
        private static string mainPath = Environment.GetEnvironmentVariable("appdata") + "/Star-Tools";
        private static string configPath = Environment.GetEnvironmentVariable("appdata") + "/Star-Tools/config";
        private static string passwordPath = Environment.GetEnvironmentVariable("appdata") + "/Star-Tools/config/pwSave.json";

        public LocalDataManager()
        {
            if (!Directory.Exists(mainPath)) Directory.CreateDirectory(mainPath);
            if (!Directory.Exists(configPath)) Directory.CreateDirectory(configPath);
        }

        public void SavePassword(string Username, string password)
        {
            if (!File.Exists(passwordPath)) File.Create(passwordPath).Close();

            PasswordSave passwordSave = new PasswordSave();
            passwordSave.Username = Username;
            passwordSave.createDate = DateTime.UtcNow;
            passwordSave.Password = password;

            File.WriteAllText(passwordPath, JsonConvert.SerializeObject(passwordSave));
        }
        public string getUsername()
        {
            JObject o = JObject.Parse(File.ReadAllText(passwordPath));

            return o.GetValue("Username").ToString();
        }
        public string getPassword()
        {
            JObject o = JObject.Parse(File.ReadAllText(passwordPath));

            return o.GetValue("Password").ToString();
        }
        public bool passwordExists()
        {
            return File.Exists(passwordPath);
        }
    }
}
