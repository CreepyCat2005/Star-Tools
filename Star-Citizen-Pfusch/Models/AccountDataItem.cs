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
        public DateTime? AccountCreatedOn;
    }
    public class ShipWatcherItem
    {
        public string localName;
        public string name;
    }
}
