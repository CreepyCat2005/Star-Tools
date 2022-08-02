using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class CoolerItem : ModuleItem
    {
        [BsonElement("cooler")]
        public CoolerDataItem data;
    }
    [BsonIgnoreExtraElements]
    public class CoolerDataItem
    {
        public int coolingRate;
        public double suppressionIRFactor, suppressionHeatFactor;
    }
}
