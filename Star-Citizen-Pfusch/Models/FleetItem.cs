using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class FleetItem
    {
        [Required]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string name { get; set; }
        [BsonElement("manufacturerData")]
        public ManufacturerItem manufacturer { get; set; }
        [BsonElement("vehicle")]
        public FleetDataItem data { get; set; }
        public FleetHullItem hull { get; set; }
        public FleetIFCSItem ifcs { get; set; }
        public int fuelCapacity { get; set; }
        public double qtFuelCapacity { get; set; }
        public FleetShieldItem shield { get; set; }
        public int? price { get; set; }
        public string localName { get; set; }
        public double cargo { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class FleetShieldItem
    {
        [BsonElement("faceType")]
        public string type { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class FleetDataItem
    {
        public string career { get; set; }
        public string role { get; set; }
        public FleetSizeItem size { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class FleetIFCSItem
    {
        public double maxSpeed { get; set; }
        public int maxAfterburnSpeed { get; set; }
        [BsonElement("angularVelocity")]
        public FleetVelocityItem velocity { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class FleetVelocityItem
    {
        [BsonElement("x")]
        public int pitch { get; set; }
        [BsonElement("y")]
        public int roll { get; set; }
        [BsonElement("z")]
        public int yaw { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class FleetHullItem
    {
        public double mass { get; set; }
        [BsonElement("hp")]
        public FleetHealthItem[] health { get; set; }
        public int hp
        {
            get
            {
                int buf = 0;
                for (int i = 0; i < health.Length; i++)
                {
                    buf += health[i].hp;
                }
                return buf;
            }
        }
    }
    [BsonIgnoreExtraElements]
    public class FleetHealthItem
    {
        public int hp { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class FleetSizeItem
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public string size
        {
            get
            {
                return $"{y} x {x} x {z}";
            }
        }
    }
    [BsonIgnoreExtraElements]
    public class ManufacturerItem
    {
        public ManufacturerDataItem data { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class ManufacturerDataItem
    {
        public string name { get; set; }
    }
}
