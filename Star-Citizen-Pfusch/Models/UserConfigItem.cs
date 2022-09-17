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
        public string ChartColor { get; set; }
        public string ChartPointColor { get; set; }
        public string SliderColor { get; set; }
        public string Theme { get; set; }
        public double TextFontSize { get; set; }
        public double MenuFontSize { get; set; }
        public double HeadlineFontSize { get; set; }
        public bool IsRainbowActive { get; set; }
        public bool IsModernShipListActive { get; set; }
        public double RainbowValue { get; set; }    
        public string BackgroundColor { get; set; }
        public string DarkBackgroundColor { get; set; }
        public string GridColumnName { get; set; }
        public string GridColumnManufacturer { get; set; }
        public string GridColumnRole { get; set; }
        public string GridColumnCareer { get; set; }
        public string GridColumnSize { get; set; }
        public string GridColumnCargo { get; set; }
        public string GridColumnMass { get; set; }
        public string GridColumnHealth { get; set; }
        public string GridColumnSpeed { get; set; }
        public string GridColumnAfterburner { get; set; }
        public string GridColumnPitch { get; set; }
        public string GridColumnYaw { get; set; }
        public string GridColumnRoll { get; set; }
        public string GridColumnHydrogenFuel { get; set; }
        public string GridColumnQuantumFuel { get; set; }
        public string GridColumnShieldType { get; set; }
        public string GridColumnPrice { get; set; }
        public string DefaultStartSize { get; set; }

    }
}
