using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Star_Citizen_Pfusch.Models
{
    public class NoteItem : ICloneable
    {
        public string id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public string LastModified { get; set; }
        public string[] Tags { get; set; }

        public object Clone()
        {
            return new NoteItem()
            {
                id = id,
                Header = Header,
                Body = Body,
                LastModified = LastModified,
                Tags = Tags
            };
        }
    }
}
