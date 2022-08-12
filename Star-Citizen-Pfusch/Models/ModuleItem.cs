using MongoDB.Bson.Serialization.Attributes;
using Star_Citizen_Pfusch.Models;
using Star_Citizen_Pfusch.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class ModuleItem
    {
        public string grade, name, description, localName, @class, _id;
        public double health, mass;
        public int size, type;
        public ModulePortLoadoutItem[] loadout { get; set; }
        public ShopItem[] shops;
    }
    [BsonIgnoreExtraElements]
    public class ModulePortLoadoutItem
    {
        public string itemPortName { get; set; }
        public string localName { get; set; }
    }
}
