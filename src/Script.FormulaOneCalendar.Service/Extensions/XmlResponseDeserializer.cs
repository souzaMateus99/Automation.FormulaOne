using System.IO;
using System.Net.Http;
using System.Xml.Serialization;
using RestEase;

namespace Script.FormulaOneCalendar.Service.Extensions
{
    public class XmlResponseDeserializer : ResponseDeserializer
    {
        public override T Deserialize<T>(string content, HttpResponseMessage response, ResponseDeserializerInfo info)
        {
            // Consider caching generated XmlSerializers
            var serializer = new XmlSerializer(typeof(T));

            using (var stringReader = new StringReader(content))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}