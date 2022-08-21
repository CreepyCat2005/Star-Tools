using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    internal class FleetyardShipItem
    {
        [JsonProperty("model.name")]
        public string ShipName { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("model.storeImageSmall")]
        public string ImageURL { get; set; }
        [JsonProperty("model.scIdentifier")]
        public string localName { get; set; }
    }
}
