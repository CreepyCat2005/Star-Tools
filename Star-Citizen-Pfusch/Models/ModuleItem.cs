using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Star_Citizen_Pfusch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class ModuleItem : ICloneable
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
        public ModuleItem[] Loadout { get; set; }

        public static void DeserializeItem(JToken token, List<ModuleItem> list)
        {
            switch (token["type"].ToString())
            {
                case "QuantumDrive":
                    list.Add(JsonConvert.DeserializeObject<QuantumDriveItem>(token.ToString()));
                    break;
                case "Cooler":
                    list.Add(JsonConvert.DeserializeObject<CoolerItem>(token.ToString()));
                    break;
                case "PowerPlant":
                    list.Add(JsonConvert.DeserializeObject<PowerPlantItem>(token.ToString()));
                    break;
                case "Shield":
                    list.Add(JsonConvert.DeserializeObject<ShieldItem>(token.ToString()));
                    break;
                case "MissileLauncher":
                    list.Add(JsonConvert.DeserializeObject<MissileRackItem>(token.ToString()));
                    break;
                case "WeaponGun":
                    list.Add(JsonConvert.DeserializeObject<WeaponItem>(token.ToString()));
                    break;
                default:
                    list.Add(JsonConvert.DeserializeObject<ModuleItem>(token.ToString()));
                    break;
            }
        }

        public object Clone()
        {
            ModuleItem item = new ModuleItem()
            {
                _id = _id,
                Description = Description,
                Grade = Grade,
                Health = Health,
                LocalName = LocalName,
                Mass = Mass,
                Name = Name,
                RequiredTags = RequiredTags,
                ShortName = ShortName,
                Size = Size,
                SubType = SubType,
                Tags = Tags,
                Type = Type
            };
            if (Loadout != null) item.Loadout = (ModuleItem[])Loadout.Clone();

            return item;
        }
    }
}
