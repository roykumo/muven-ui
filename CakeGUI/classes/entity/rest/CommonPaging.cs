using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity.rest
{
    public class TCommonPaging<T>
    {
        [DeserializeAs(Name = "data")]
        public List<T> Data { get; set; }

        [DeserializeAs(Name = "page")]
        public Int32 Page { get; set; }

        [DeserializeAs(Name = "rowPerPage")]
        public Int32 RowPerPage { get; set; }

        [DeserializeAs(Name = "totalData")]
        public long TotalData { get; set; }
    }
}
