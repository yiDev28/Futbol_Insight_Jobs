using Futbol_Insight_Jobs.Models;

namespace Futbol_Insight_Jobs.Services.ApiService
{
    public interface IApiService
    {
         Task<ResultModel<List<ApiCountry>>> GetCountries();
    }
}
