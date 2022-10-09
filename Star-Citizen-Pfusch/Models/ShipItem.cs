using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Star_Citizen_Pfusch.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class ShipItem
    {
        public string _id { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string Career { get; set; }
        public string LocalName { get; set; }
        public string[] Tags { get; set; }
        public string[] RequiredTags { get; set; }
        public int Size { get; set; }
        public int Grade { get; set; }
        public double Cargo { get; set; }
        public double Mass { get; set; }
        public StatusEnum Status { get; set; }
        public double HydrogenFuelCapacity { get; set; }
        public double QuantumFuelCapacity { get; set; }
        public ShipSize ShipSize { get; set; }
        public LoadoutItem[] Loadout { get; set; }
        public PureShopDataItem[] Shops { get; set; }
        public int RealPrice { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
