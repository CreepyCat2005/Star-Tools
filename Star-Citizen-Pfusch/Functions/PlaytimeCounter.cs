using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace Star_Citizen_Pfusch.Functions
{
    class PlaytimeCounter
    {
        private static int waitTime;
        private static int playtime;
        public static void start(int interval)
        {
            waitTime = interval;
            playtime = 0;
            Timer timer = new Timer(interval);
            timer.Elapsed += Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static async void Elapsed(object source, ElapsedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("StarCitizen");
            if (processes.Length > 0 || playtime != 0)
            {
                playtime += waitTime / (1000 * 60);
                Debug.WriteLine("Playtime: " + playtime);

                if (playtime >= 10 || processes.Length == 0)
                {
                    await savePlaytime();
                }
            }
        }
        public static async Task savePlaytime()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Content = new StringContent(JsonConvert.SerializeObject(new Models.AccountDataItem() { Playtime = playtime, SessionToken = Config.SessionToken }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(Config.URL + "/AccountData", request.Content);
                Debug.WriteLine(await request.Content.ReadAsStringAsync());
                Debug.WriteLine("Send playtime to server!");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    playtime = 0;
                    Debug.WriteLine("Server replied OK!");
                }
            }
        }
    }
}
