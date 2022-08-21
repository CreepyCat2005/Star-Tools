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
using System.Data.SQLite;

namespace Star_Citizen_Pfusch.Functions
{
    class LocalDataManager
    {
        private static string mainPath = Environment.GetEnvironmentVariable("appdata") + "/Star-Tools";
        private static string configPath = Environment.GetEnvironmentVariable("appdata") + "/Star-Tools/config";
        private static string passwordPath = Environment.GetEnvironmentVariable("appdata") + "/Star-Tools/config/pwSave.json";
        private static SQLiteConnection SqLite;

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
        public static string GetFleetyardToken()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Roaming\\Mozilla\\Firefox\\Profiles");
            DirectoryInfo[] files = directoryInfo.GetDirectories();
            string path = files.Where(o => DirSize(o) == files.Select(o => DirSize(o)).Max()).ToList()[0].FullName;

            SqLite = new SQLiteConnection("Data Source=" + path + "\\cookies.sqlite");
            SqLite.Open();

            string token = "";

            var command = SqLite.CreateCommand();
            command.CommandText = "SELECT value FROM moz_cookies WHERE host = '.fleetyards.net'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                token = reader.GetString(0);
            }
            return token;
        }
        private static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }
    }
}
