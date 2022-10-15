using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    internal class ReportItem
    {
        public string SessionToken { get; set; }
        public AccountItem AccountItem { get; set; }
        public string PageName { get; set; }
        public DateTime Time { get; set; }
        public string ReportMessage { get; set; }
    }
}
