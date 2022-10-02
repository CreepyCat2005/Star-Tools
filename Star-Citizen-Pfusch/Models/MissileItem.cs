
using MongoDB.Bson.Serialization.Attributes;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class MissileItem : ModuleItem
    {
        public int explosionSafetyDistance { get; set; }
        public double igniteTime { get; set; }
        public double collisionDelayTime { get; set; }
        public double armTime { get; set; }
        public double maxLifetime { get; set; }
        public double projectileProximity { get; set; }
        public MissileExplosionItem explosion { get; set; }
    }
    public class MissileExplosionItem
    {
        public int holeSize { get; set; }
        public int terrainHoleSize { get; set; }
        public double maxRadius { get; set; }
        public double maxPhysRadius { get; set; }
        public double DamagePhysical { get; set; }
        public double DamageEnergy { get; set; }
        public double DamageDistortion { get; set; }
        public double DamageThermal { get; set; }
        public double DamageBiochemical { get; set; }
        public double DamageStun { get; set; }
        public double minRadius { get; set; }
        public double minPhysRadius { get; set; }
    }
}
