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
        public double SuppressionIRFactor { get; set; }
        public double SuppressionHeatFactor { get; set; }
        public double CoolingRate { get; set; }
    }
}
