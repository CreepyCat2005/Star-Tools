using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    public class ShipDataItem
    {
        public int crewSize;
        public string career, role;
        public double inventoryCapacity;
        public ShipSizeItem size;
    }
    public class ShipSizeItem
    {
        public double x, y, z;
    }
    public class ShipHullItem
    {
        public double mass;
    }
}
