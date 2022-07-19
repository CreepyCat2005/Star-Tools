﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class ShipItem
    {
        public string _id;
        public int health, maxLifetimeHours, size;
        public string type, subtype, grade, name, description, localName, status;
        public double cargo, qtFuelCapacity, fuelCapacity;
        public ShipDataItem data;
        public ShipHullItem hull;
    }
}
