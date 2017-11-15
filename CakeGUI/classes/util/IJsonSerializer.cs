using RestSharp.Deserializers;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeGUI.classes.util
{
    public interface IJsonSerializer : ISerializer, IDeserializer
    {

    }
}
