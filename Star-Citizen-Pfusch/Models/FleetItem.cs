using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class FleetItem
    {
        public string _id { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string Career { get; set; }
        public string[] Tags { get; set; }
        public string[] RequiredTags { get; set; }
        public int Size { get; set; }
        public int Health { get; set; }
        public int Grade { get; set; }
        public double Cargo { get; set; }
        public double Mass { get; set; }
        public int Price { get; set; }
        public double HydrogenFuelCapacity { get; set; }
        public double QuantumFuelCapacity { get; set; }
        public ShipSize ShipSize { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
    public class ShipSize
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
    }
}
