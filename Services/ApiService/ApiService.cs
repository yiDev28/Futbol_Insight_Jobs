using Futbol_Insight_Jobs.Data;
using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Tools;
using LogServiceYiDev.Models;
using System.Collections.Generic;
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

        public async Task<ResultModel<List<ApiCountry>>> GetCountries()
        {
            ResultModel<List<ApiCountry>> result = new ResultModel<List<ApiCountry>>();
            try
            {
                var _res = await _apiContext.GetApiAllSport("Countries");
                // Leer el contenido de la respuesta como cadena
                var responseBody = await _res.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiCountryResponse>(responseBody);

                if (apiResponse.success != 1)
                {
                    throw new Exception("Error en la respuesta del api - Countries");
                }
                result.Code = 5200;
                result.Message = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5200).UserMessage;
                result.Data = apiResponse.result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }
    }
}
