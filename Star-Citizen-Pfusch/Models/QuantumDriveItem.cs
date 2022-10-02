using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class QuantumDriveItem : ModuleItem
    {
        public double quantumFuelRequirement { get; set; }
        public double stageOneAccelRate { get; set; }
        public double stageTwoAccelRate { get; set; }
        public double driveSpeed { get; set; }
        public double cooldownTime { get; set; }
        public int disconnectRange { get; set; }
        public int engageSpeed { get; set; }
    }
}
