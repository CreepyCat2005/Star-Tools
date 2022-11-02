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
        public string GetAmmo
        {
            get
            {
                if (initialAmmoCount == 0) return "∞";
                else return initialAmmoCount.ToString();
            }
        }
    }
    public class WeaponDamageItem
    {
        public double DamagePhysical { get; set; }
        public double DamageEnergy { get; set; }
        public double DamageDistortion { get; set; }
        public double DamageThermal { get; set; }
        public double DamageBiochemical { get; set; }
        public double DamageStun { get; set; }
        public double alphaMin { get; set; }
        public double alphaMax { get; set; }
        public double fireRateMin { get; set; }
        public double fireRateMax { get; set; }
    }
}
