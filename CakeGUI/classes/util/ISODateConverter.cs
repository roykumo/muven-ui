using Newtonsoft.Json;
using RestSharp.Serializers;
using System.IO;
using RestSharp.Deserializers;
using Newtonsoft.Json.Converters;

namespace CakeGUI.classes.util
{
    public class ISODateConverter : IsoDateTimeConverter
    {
    
        public ISODateConverter()
        {
            base.DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
        }
        
    }
}
