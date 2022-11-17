using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

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
        public static string GetRSICookieString()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Roaming\\Mozilla\\Firefox\\Profiles");
            DirectoryInfo[] files = directoryInfo.GetDirectories();
            string path = "";
            while (path.Equals(""))
            {
                try
                {
                    path = files.Where(o => DirSize(o) == files.Select(o => DirSize(o)).Max()).ToList()[0].FullName;
                } 
                catch { }
            }

            SqLite = new SQLiteConnection("Data Source=" + path + "\\cookies.sqlite");
            SqLite.Open();

            var nameList = new List<string>();
            var valueList = new List<string>();

            var command = SqLite.CreateCommand();
            command.CommandText = "SELECT name,value FROM moz_cookies WHERE host LIKE '%robertsspaceindustries%'";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                nameList.Add(reader.GetString(0));
                valueList.Add(reader.GetString(1));
            }

            string token = "";
            for (int i = 0; i < valueList.Count; i++)
            {
                token += nameList[i] + "=" + valueList[i] + "; ";
            }

            return token;
        }
        public static PledgeItem[] GetPledgeItems()
        {
            if (Config.BrowserType.Equals("")) return null;
            string cookieString = "";

            switch (Config.BrowserType)
            {
                case Models.Enums.BrowserEnum.Firefox:
                    cookieString = Config.RSICookieString;
                    break;
                default:
                    return null;
            }
            HttpClient client = new HttpClient();

            List<PledgeItem> nameList = new List<PledgeItem>();
            PledgeItem[] pledgeItems;
            int counter = 1;
            do
            {
                pledgeItems = GetPledgeItems(client, cookieString, counter);
                nameList.AddRange(pledgeItems);

                counter++;
            }
            while (pledgeItems.Length != 0);

            return nameList.ToArray();
        }
        private static PledgeItem[] GetPledgeItems(HttpClient client, string cookieString, int page)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://robertsspaceindustries.com/account/pledges?page={page}&pagesize=100");
            request.Headers.Add("Cookie", cookieString);
            HttpResponseMessage response = client.SendAsync(request).Result;
            string res = response.Content.ReadAsStringAsync().Result;
            res = Regex.Replace(res, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            string[] lineArray = res.Split("\n");
            List<PledgeItem> nameList = new List<PledgeItem>();

            for (int i = 0; i < lineArray.Length; i++)
            {
                if (!lineArray[i].Contains("<input type=\"hidden\" class=\"")) continue;

                PledgeItem item = new PledgeItem()
                {
                    id = int.Parse(GetValue(lineArray[i])),
                    name = GetValue(lineArray[i + 1]),
                    value = GetValue(lineArray[i + 2]),
                    configValue = GetValue(lineArray[i + 3]),
                    curreny = GetValue(lineArray[i + 4]),
                    lastAlpha = int.Parse(GetValue(lineArray[i + 5]))
                };

                for (int x = 0; x < 30; x++)
                {
                    if (lineArray[i - x].Contains("style=\"background-image:url('"))
                    {
                        item.ImagePath = lineArray[i - x].Substring(lineArray[i - x].IndexOf("('") + 2, lineArray[i - x].LastIndexOf("')") - lineArray[i - x].IndexOf("('") - 2);
                    }
                    else if (lineArray[i + x].Contains("<label>Created:</label>")) item.createdAt = lineArray[i + x + 1].Trim();
                    else if (lineArray[i + x].Contains("<label>Contains:</label>")) item.contains = lineArray[i + x + 1].Trim();
                }

                i += 8;
                nameList.Add(item);
            }

            return nameList.ToArray();
        }


        private static string GetValue(string line)
        {
            return line.Substring(line.IndexOf("value=\"") + 7, line.IndexOf("\">", line.IndexOf("value=\"") + 7) - line.IndexOf("value=\"") - 7);
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
