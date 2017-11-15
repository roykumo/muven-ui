using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity.rest
{
    public class TCommonResponsePaging<T>
    {
        [DeserializeAs(Name = "responseStatus")]
        public CommonStatus ResponseStatus { get; set; }

        [DeserializeAs(Name = "paging")]
        public TCommonPaging<T> Paging { get; set; }
    }
}
