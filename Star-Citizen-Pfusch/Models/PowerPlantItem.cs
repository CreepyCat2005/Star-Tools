using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Star_Citizen_Backend.Models
{
    [BsonIgnoreExtraElements]
    public class PowerPlantItem : ModuleItem
    {
        [BsonElement("power")]
        public PowerPlantDataItem data;
    }
    [BsonIgnoreExtraElements]
    public class PowerPlantDataItem
    {
        public int powerBase, safeguardPriority, warningDisplayTime;
        public bool isThrottleable, isOverclockable;
        public double timeToReachDrawRequest, powerDraw, overclockThresholdMin, overclockThresholdMax, overpowerPerformance, overclockPerformance, powerToEM, decayRateOfEM, warningDelayTime;
    }
}
