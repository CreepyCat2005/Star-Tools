using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    class FundingItem
    {
        public string totalRaised;
        public int totalPlayer;
        public IEnumerable<DateTime> date;
        public IEnumerable<int> value;
    }
}
