using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Star_Citizen_Backend.Models
{
    [BsonIgnoreExtraElements]
    public class ModuleItem
    {
        public string grade, name, description, localName, @class;
        public double health, mass;
        public int size, type;
    }
}
