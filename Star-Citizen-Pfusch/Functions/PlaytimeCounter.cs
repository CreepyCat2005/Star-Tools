﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Star_Citizen_Pfusch.Functions
{
    class PlaytimeCounter
    {
        private static int counter, waitTime;
        private static double playtime;
        public PlaytimeCounter(int interval)
        {
            waitTime = interval;
            playtime = 0;
            counter = 0;
            Timer timer = new Timer(interval);
            timer.Elapsed += Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static async void Elapsed(object source, ElapsedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("notepad");
            if (processes.Length > 0)
            {
                playtime += waitTime / 1000.0;

            if (counter >= 10)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();
                        request.Content = new StringContent(JsonConvert.SerializeObject(new Models.AccountData() { Playtime = playtime, SessionToken = Config.SessionToken }),Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PutAsync(Config.URL + "/AccountData", request.Content);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            playtime = 0;
                counter = 0;
            }
                    }
                }
            else
            {
                counter++;
            }
            }
            Debug.WriteLine(playtime);
        }
    }
}