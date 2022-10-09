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
        public double quantumFuelRequirement
        {
            get
            {
                return quantumFuelRequirementINTERN * 1000;
            }
            set
            {
                quantumFuelRequirementINTERN = value;
            }
        }
        public double stageOneAccelRate { get; set; }
        public double stageTwoAccelRate { get; set; }
        public double driveSpeed
        {
            get
            {
                return this.driveSpeedINTERN / 1000;
            }
            set
            {
                this.driveSpeedINTERN = value;
            }
        }
        public double cooldownTime { get; set; }
        public int disconnectRange { get; set; }
        public int engageSpeed { get; set; }

        private double driveSpeedINTERN;
        private double quantumFuelRequirementINTERN;
    }
}
