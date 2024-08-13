using Futbol_Insight_Jobs.Services.Country;

namespace Futbol_Insight_Jobs.Models
{
    public class ApiCountryResponse
    {
        public int success { get; set; }
        public List<ApiCountry> result { get; set; }
    }

    public class ApiCountry
    {
        public int country_key { get; set; }
        public string country_name { get; set; }
        public string country_iso2 { get; set; }
        public string country_logo { get; set; }
    }
}
