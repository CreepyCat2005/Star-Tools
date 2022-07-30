using MongoDB.Bson.Serialization.Attributes;

namespace Star_Citizen_Backend.Models
{
    [BsonIgnoreExtraElements]
    public class ShieldItem : ModuleItem
    {
        [BsonElement("shield")]
        public ShieldDataItem data;
    }
    [BsonIgnoreExtraElements]
    public class ShieldDataItem
    {
        public int maxShieldHealth, regenExcessMax, regenExcessChargePerSec, regenExcessUseCooldown, downedRegenDelay, damagedRegenDelay;
        public double maxShieldRegen, decayRatio;
        public ShieldAbsorptionItem absorption;
    }
    [BsonIgnoreExtraElements]
    public class ShieldAbsorptionItem
    {
        public double physicalMin, physicalMax, energyMin, energyMax, distortionMin, distortionMax, thermalMin, thermalMax, biochemicalMin, biochemicalMax, stunMin, stunMax;
    }
}
