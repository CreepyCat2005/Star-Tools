
using MongoDB.Bson.Serialization.Attributes;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class MissileItem : ModuleItem
    {
        [BsonElement("missile")]
        public MissileDataItem data { get; set; }
        [BsonElement("manufacturerData")]
        public ManufacturerItem manufacturer { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class MissileDataItem
    {
        public int explosionSafetyDistance { get; set; }
        public double projectileProximity { get; set; }
        public double armTime { get; set; }
        public double igniteTime { get; set; }
        public double collisionDelayTime { get; set; }
        public int irSignalMinValue { get; set; }
        public int irSignalMaxValue { get; set; }
        public int irSignalRiseRate { get; set; }
        public int irSignalDecayRate { get; set; }
        public string trackingSignalType { get; set; }
        public int lockSignalAmplifier { get; set; }
        public double lockTime { get; set; }
        public int lockingAngle { get; set; }
        public double minRatioForLock { get; set; }
        public double lockIncreaseRate { get; set; }
        public double lockDecreaseRate { get; set; }
        public int lockRangeMin { get; set; }
        public int lockRangeMax { get; set; }
        public int lockResolutionRadius { get; set; }
        public double signalResilienceMin { get; set; }
        public double signalResilienceMax { get; set; }
        public int linearSpeed { get; set; }
        public int fuelTankSize { get; set; }
        public int maxLifetime { get; set; }
        public double minRadius { get; set; }
        public double maxRadius { get; set; }
        public double minPhysRadius { get; set; }
        public double maxPhysRadius { get; set; }
        public string hitType { get; set; }
        public DamageItem damage { get; set; }
    }
}
