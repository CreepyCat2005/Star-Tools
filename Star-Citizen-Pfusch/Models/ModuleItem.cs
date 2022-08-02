using MongoDB.Bson.Serialization.Attributes;
using Star_Citizen_Pfusch.Models;
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
        public ShopItem[] shops;
    }
}
