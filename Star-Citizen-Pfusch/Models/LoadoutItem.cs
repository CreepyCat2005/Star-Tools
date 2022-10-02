using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    public class LoadoutItem
    {
        public string itemPortName { get; set; }
        public string localName { get; set; }
        public LoadoutItem[] Loadout { get; set; }
    }
}
