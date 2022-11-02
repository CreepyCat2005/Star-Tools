using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Star_Citizen_Pfusch.Models
{
    internal class PledgeItem
    {
        public int id, lastAlpha;
        public string name, value, configValue, curreny, createdAt, contains;
        private string _imagePath;
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                if (value.Contains("https://media.robertsspaceindustries.com"))
                {
                    _imagePath = value;
                }
                else
                {
                    _imagePath = "https://robertsspaceindustries.com" + value;
                }
            }
        }
    }
}
