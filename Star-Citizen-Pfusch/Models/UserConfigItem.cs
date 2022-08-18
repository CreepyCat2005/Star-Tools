using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    internal class UserConfigItem
    {
        public string TextColor { get; set; }
        public string MenuColor { get; set; }
        public string HeadlineColor { get; set; }
        public string Theme { get; set; }
        public double TextFontSize { get; set; }
        public double MenuFontSize { get; set; }
        public double HeadlineFontSize { get; set; }
    }
}
