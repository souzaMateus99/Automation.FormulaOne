using System;
using System.Threading.Tasks;
using RestEase;
using Script.FormulaOneCalendar.Service;
using Script.FormulaOneCalendar.Service.Clients;
using Script.FormulaOneCalendar.Service.Extensions;
using Script.FormulaOneCalendar.Service.Interfaces;

namespace Script.FormulaOneCalendar
{
    class Program
    {
        private const string ERGAST_FORMULA_ONE_URL = "http://ergast.com/api/f1/";
        
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            IErgastClient ergastClient = new RestClient(ERGAST_FORMULA_ONE_URL)
            {
                ResponseDeserializer = new XmlResponseDeserializer()
            }.For<IErgastClient>();

            IRaceService service = new RaceService(ergastClient);

            var schedules = await service.GetScheduledRacesAsync(2021);
        }
    }
}
