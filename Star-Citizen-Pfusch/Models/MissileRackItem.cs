using MongoDB.Bson.Serialization.Attributes;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class MissileRackItem : ModuleItem
    {
        public double LaunchDelay { get; set; }
        public double DetachVelocityRight { get; set; }
        public double DetachVelocityForward { get; set; }
        public double DetachVelocityUp { get; set; }
        public string FragReadyUp { get; set; }
        public string FragStowAway { get; set; }
        public MissileRackPortItem[] Ports { get; set; }
    }
    public class MissileRackPortItem
    {
        public string DisplayName { get; set; }
        public string LocalName { get; set; }
        public string Type { get; set; }
        public string[] Tags { get; set; }
        public string[] RequiredTags { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
    }
}
