using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    class ShipItem
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int Mass, Cargo, Hp, Price;
        public string LocalName, Role, Career, Description, ShieldType, Status;
        public double Length, Width, Height;
    }
}
