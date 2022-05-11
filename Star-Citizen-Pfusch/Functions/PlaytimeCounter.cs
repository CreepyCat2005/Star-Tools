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
        private static int counter;
        private static long playtime, startedPlaying;
        public PlaytimeCounter(int interval)
        {
            playtime = 0;
            counter = 0;
            Timer timer = new Timer(interval);
            timer.Elapsed += Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void Elapsed(object source, ElapsedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("starcitizen");
            if (processes.Length > 0)
            {
                if (playtime == 0) startedPlaying = DateTime.UtcNow.Millisecond;
                else playtime += DateTime.UtcNow.Millisecond - playtime;
            }
            else
            {
                playtime = 0;
                startedPlaying = 0;
            }
            Debug.WriteLine(playtime);

            if (counter >= 10)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage();

                }

                counter = 0;
            }
            else
            {
                counter++;
            }
        }
    }
}
