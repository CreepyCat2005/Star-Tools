using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class PowerPlantItem : ModuleItem
    {
        public int PowerBase { get; set; }
        public int SafeguardPriority { get; set; }
        public int WarningDisplayTime { get; set; }
        public bool IsThrottleable { get; set; }
        public bool IsOverclockable { get; set; }
        public double PowerToEM { get; set; }
        public double DecayRateOfEM { get; set; }
        public double WarningDelayTime { get; set; }
        public double TimeToReachDrawRequest { get; set; }
        public double PowerDraw { get; set; }
    }
}
