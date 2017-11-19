using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity.rest
{
    public class CommonStatus
    {
        [DeserializeAs(Name = "responseCode")]
        public string ResponseCode { get; set; }
        [DeserializeAs(Name = "responseDesc")]
        public string ResponseDesc { get; set; }
    }
}
