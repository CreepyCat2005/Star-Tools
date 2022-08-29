using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    class PublicDataItem
    {
        public string _id { get; set; }
        public string gameVersion;
        public string PTUStatus;
        public ShipItem dailyShip;
    }
}