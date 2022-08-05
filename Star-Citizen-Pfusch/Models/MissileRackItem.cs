using MongoDB.Bson.Serialization.Attributes;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class MissileRackItem : ModuleItem
    {
        public MissileRackPortItem[] ports { get; set; }
        [BsonElement("missileRack")]
        public MissilRackDataItem data { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class MissilRackDataItem
    {
        public double launchDelay { get; set; }
        public double detachVelocityRight { get; set; }
        public double detachVelocityForward { get; set; }
        public double detachVelocityUp { get; set; }
        public bool igniteOnPylon { get; set; }

    }
    [BsonIgnoreExtraElements]
    public class MissileRackPortItem
    {
        public int minSize { get; set; }
        public int maxSize { get; set; }
        public MissilRackItemTypeItem[] itemTypes { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class MissilRackItemTypeItem
    {
        public string type { get; set; }
        public string subType { get; set; }
    }
}
