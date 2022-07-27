using Star_Citizen_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    class QuantumDriveItem : ModuleItem
    {
        public int disconnectRange;
        public double quantumFuelRequirement;
        public QuantumDriveDataItem data;
    }

    class QuantumDriveDataItem
    {
        public int driveSpeed, stageOneAccelRate, stageTwoAccelRate, engageSpeed, interdictionEffectTime, calibrationRate, minCalibrationRequirement,
            maxCalibrationRequirement, calibrationProcessAngleLimit, calibrationWarningAngleLimit, calibrationDelayInSeconds;
        public double cooldownTime, spoolUpTime;
    }
}