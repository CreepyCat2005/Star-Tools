using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class WeaponItem : ModuleItem
    {
        public int idealCombatRange { get; set; }
        public double supplementaryFireTime { get; set; }
        public bool isAllowedInGreenZones { get; set; }
        public WeaponAmmoItem Ammo { get; set; }
        public WeaponDamageItem Damage { get; set; }
    }
    public class WeaponAmmoItem
    {
        public int initialAmmoCount { get; set; }
        public int maxAmmoCount { get; set; }
    }
    public class WeaponDamageItem
    {
        public int DamagePhysical { get; set; }
        public int DamageEnergy { get; set; }
        public int DamageDistortion { get; set; }
        public int DamageThermal { get; set; }
        public int DamageBiochemical { get; set; }
        public int DamageStun { get; set; }
    }
}
