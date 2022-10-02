using MongoDB.Bson.Serialization.Attributes;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class ShieldItem : ModuleItem
    {
        public int RegenExcessMax { get; set; }
        public int RegenExcessChargePerSec { get; set; }
        public int RegenExcessUseCooldown { get; set; }
        public int DamagedRegenDelay { get; set; }
        public double DecayRatio { get; set; }
        public double MaxShieldRegen { get; set; }
        public double MaxShieldHealth { get; set; }
        public double DownedRegenDelay { get; set; }
    }
}
