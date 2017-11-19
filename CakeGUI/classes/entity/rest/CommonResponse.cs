using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity.rest
{
    public class TCommonResponse<T>
    {
        [DeserializeAs(Name = "responseStatus")]
        public CommonStatus ResponseStatus { get; set; }

        [DeserializeAs(Name = "data")]
        public T Data { get; set; }
    }
}
