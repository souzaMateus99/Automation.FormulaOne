using RestEase;

namespace Script.FormulaOneCalendar.Service.Clients
{
    public interface IFormulaOneHeaders
    {
        [Header("User-Agent")]
        public string UserAgent { get; set; }
    }
}