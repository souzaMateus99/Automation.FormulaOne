using System;
using RestEase;
using Script.FormulaOneCalendar.Service.Extensions;

namespace Script.FormulaOneCalendar.Service.Factories
{
    public static class RestEaseFactory
    {
        public static T CreateClientWithXmlResponse<T>(Uri url)
        {
            return new RestClient(url)
            {
                ResponseDeserializer = new XmlResponseDeserializer()
            }.For<T>();   
        }

        public static T CreateClient<T>(Uri url)
        {
            return new RestClient(url).For<T>();
        }
    }
}