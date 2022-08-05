using MongoDB.Bson.Serialization.Attributes;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class MiningLaserItem : ModuleItem
    {
        public MiningLaserWeaponItem weapon { get; set; }
        public MiningLaserLaserItem miningLaser { get; set; }

    }
    [BsonIgnoreExtraElements]
    public class MiningLaserWeaponItem
    {
        public int idealCombatRange { get; set; }
        public MiningLaserDataItem data { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class MiningLaserDataItem
    {
        public int energyRate { get; set; }
        public int fullDamageRate { get; set; }
        public int zeroDamageRate { get; set; }
        public int heatPerSecond { get; set; }
        public double hitRadius { get; set; }
        public MiningLaserDamageItem damage { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class MiningLaserDamageItem
    {
        public int damagePhysical { get; set; }
        public int damageEnergy { get; set; }
        public int damageDistortion { get; set; }
        public int damageThermal { get; set; }
        public int damageBiochemical { get; set; }
        public int damageStun { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class MiningLaserLaserItem
    {
        public int throttleLerpSpeed { get; set; }
        public double resistanceModifier { get; set; }
        public int laserInstability { get; set; }
        public int optimalChargeWindowSizeModifier { get; set; }
        public int shatterdamageModifier { get; set; }
        public int optimalChargeWindowRateModifier { get; set; }
        public int catastrophicChargeWindowRateModifier { get; set; }

    }
}
