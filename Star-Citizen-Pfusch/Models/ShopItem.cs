using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    public class ShopItem
    {
        public string name, location;
        public ShopDataItem[] inventory;
    }
    public class ShopDataItem
    {
        public string localName, name;
        public int basePrice, price;
    }
    public class PureShopDataItem
    {
        public string name, location;
        public int price;
    }
}