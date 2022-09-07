using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models.Enums
{
    public enum ModuleTypeEnum
    {
        Quantum_Drive, // 0
        Power_Plant,   // 1
        Weapon,        // 2
        Missile,       // 3
        Missile_Rack,  // 4
        Emp,           // 5
        Qed,           // 6
        Shield,        // 7
        Cooler,        // 8
        Paint,         // 9
        Mining,        // 10
        Mounting,      // 11
        Turret,        // 12
        Unknown        // 13

    }
    static class ModuleTypeEnumMethods
    {
        public static string ConvertToString(this ModuleTypeEnum @enum)
        {
            switch (@enum)
            {
                case ModuleTypeEnum.Quantum_Drive:
                    return "Quantum Drive";
                case ModuleTypeEnum.Power_Plant:
                    return "Power Plant";
                case ModuleTypeEnum.Weapon:
                    return "Weapon";
                case ModuleTypeEnum.Missile:
                    return "Missile";
                case ModuleTypeEnum.Missile_Rack:
                    return "Missile Rack";
                case ModuleTypeEnum.Emp:
                    return "Emp";
                case ModuleTypeEnum.Qed:
                    return "Qed";
                case ModuleTypeEnum.Shield:
                    return "Shield";
                case ModuleTypeEnum.Cooler:
                    return "Cooler";
                case ModuleTypeEnum.Paint:
                    return "Paint";
                case ModuleTypeEnum.Mining:
                    return "Mining";
                case ModuleTypeEnum.Mounting:
                    return "Mounting";
                case ModuleTypeEnum.Turret:
                    return "Turret";
                case ModuleTypeEnum.Unknown:
                    return "Unknown";
                default:
                    return "Unknown";
            }
        }
    }
}
