using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    public class MountingItem : ModuleItem
    {
        public MountingDataItem[] ports { get; set; }
    }
    public class MountingDataItem
    {
        public int minSize { get; set; }
        public int maxSize { get; set; }
    }
}
