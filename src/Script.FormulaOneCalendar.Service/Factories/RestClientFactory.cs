using RestEase;
using Script.FormulaOneCalendar.Service.Extensions;

namespace Script.FormulaOneCalendar.Service.Factories
{
    public static class RestEaseFactory
    {
        public static T CreateClientWithXmlResponse<T>(string url)
        {
            return new RestClient(url)
            {
                ResponseDeserializer = new XmlResponseDeserializer()
            }.For<T>();   
        }
    }
}