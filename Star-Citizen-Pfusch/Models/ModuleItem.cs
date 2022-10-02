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
        public string _id { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LocalName { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public string[] RequiredTags { get; set; }
        public int Size { get; set; }
        public int Grade { get; set; }
        public double Health { get; set; }
        public double Mass { get; set; }
    }
}
