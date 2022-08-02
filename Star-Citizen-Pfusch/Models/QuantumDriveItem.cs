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
        [BsonElement("qdrive")]
        public QuantumDriveDataItem data;
    }
    [BsonIgnoreExtraElements]
    public class QuantumDriveDataItem
    {
        public double quantumFuelRequirement;
        public int disconnectRange;
        public QuantumDriveParamsItem @params;
    }
    [BsonIgnoreExtraElements]
    public class QuantumDriveParamsItem
    {
        public int driveSpeed, engageSpeed, interdictionEffectTime, calibrationRate, minCalibrationRequirement, maxCalibrationRequirement, calibrationProcessAngleLimit, calibrationWarningAngleLimit;
        public double calibrationDelayInSeconds, spoolUpTime, cooldownTime, stageOneAccelRate, stageTwoAccelRate;
    }
}
