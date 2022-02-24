using System;
using Newtonsoft.Json;
using RestEase;
using Script.FormulaOneCalendar.Service.Extensions;

namespace Script.FormulaOneCalendar.Service.Factories
{
    public static class RestEaseFactory
    {
        public static T CreateClient<T>(Uri url, JsonSerializerSettings jsonSerializerSettings)
        {
            return new RestClient(url)
            {
                JsonSerializerSettings = jsonSerializerSettings
            }.For<T>();
        }
    }
}