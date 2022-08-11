using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    [BsonIgnoreExtraElements]
    public class FundingItem
    {
        public long funds { get; set; }
        public int fans { get; set; }
        public FundingDataItem[] day { get; set; }
        public FundingDataItem[] week { get; set; }
        public FundingDataItem[] month { get; set; }
    }
    public class FundingDataItem
    {
        public long gross { get; set; }
        public string axis { get; set; }
    }
}
