using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Star_Citizen_Pfusch.Models
{
    class AccountItem
    {
        public string Username, Email, Password, Salt, ProductKey;
        public AccountDataItem AccountData;
    }
}
