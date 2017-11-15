using Newtonsoft.Json;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.entity
{
    public class ProductTypeEntity
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
        [JsonProperty("expiration")]
        public bool Expiration { get; set; }
    }
}
