using Futbol_Insight_Jobs.Data;
using Futbol_Insight_Jobs.Models;
using System.Text.Json;
using System.Web;

namespace Futbol_Insight_Jobs.Services.ApiService
{
    public class ApiService : IApiService
    {
        private readonly ApiContext _apiContext;

        public ApiService(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task<List<ApiCountry>> GetCountries()
        {
            try
            {
                var _res = await _apiContext.GetApiAllSport("Countries");
                // Leer el contenido de la respuesta como cadena
                var responseBody = await _res.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiCountryResponse>(responseBody);

                if (apiResponse.success != 1 )
                {
                    throw new Exception("error en la respuesta del api");
                }
                return apiResponse.result;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
