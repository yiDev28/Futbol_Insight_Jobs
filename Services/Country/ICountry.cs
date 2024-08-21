using Futbol_Insight_Jobs.Models;

namespace Futbol_Insight_Jobs.Services.Country
{
    public interface ICountry
    {
        Task<ResultModel<bool>> SyncCountries(List<CountryModel> country);
    }
}
