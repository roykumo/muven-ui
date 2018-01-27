using Newtonsoft.Json;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class ProductCategoryEntity
    {
        //[DeserializeAs(Name = "id")]
        [JsonProperty("id")]
        public string Id { get; set; }
        //[DeserializeAs(Name = "code")]
        [JsonProperty("code")]
        public string Code { get; set; }
        //[DeserializeAs(Name = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }
        //[DeserializeAs(Name = "expiration")]
        [JsonProperty("parent")]
        public ProductCategoryEntity Parent { get; set; }
        [JsonProperty("type")]
        public ProductTypeEntity Type { get; set; }
        [JsonProperty("orderNo")]
        public Int32 OrderNo { get; set; }
    }
}
