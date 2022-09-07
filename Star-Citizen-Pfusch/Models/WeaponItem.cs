using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    public class WeaponItem : ModuleItem
    {
        public WeaponDataItem weapon;
    }
    public class WeaponDataItem
    {
        public WeaponDamageItem damage;
    }
    public class WeaponDamageItem
    {
        public double alphaMax, alphaMin, fireRateMax, fireRateMin;
    }

}
