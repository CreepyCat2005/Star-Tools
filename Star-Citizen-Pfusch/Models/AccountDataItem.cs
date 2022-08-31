using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    class AccountDataItem
    {
        public int? Playtime;
        public string SessionToken;
        public int[] PlaytimeHistory;
        public List<ShipWatcherItem> ShipsOnWatcher;
    }
    public class ShipWatcherItem
    {
        public string _id;
        public string name;
    }
}
