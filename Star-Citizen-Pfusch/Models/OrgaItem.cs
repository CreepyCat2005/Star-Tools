using System;

namespace Star_Citizen_Pfusch.Models
{
    public class OrgaItem : ICloneable
    {
        public string Name, ShortName;
        public string WebsiteURL { get; set; }
        public string DiscordURL { get; set; }
        public string Ts3URL { get; set; }
        public DateTime RegisteredAt;

        public object Clone()
        {
            return (OrgaItem)MemberwiseClone();
        }
    }
}
