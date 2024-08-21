
using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Services.ApiService;
using Futbol_Insight_Jobs.Services.Country;
using Futbol_Insight_Jobs.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Futbol_Insight_Jobs.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountry _countryService;
        private readonly IApiService _apiService;
        private readonly Utilities _utilities;


        public CountryController(ICountry countryService, IApiService apiService, Utilities utilities)
        {
            _countryService = countryService;
            _apiService = apiService;
            _utilities = utilities;
        }
        #region Sincronizar Paises
        [HttpGet]
        [Route("Refresh")]
        public async Task<IActionResult> RefrescarPaises()
        {
            ResultModel<bool> result = new ResultModel<bool>();
            try
            {
                var _apiCountries = await _apiService.GetCountries();

                if (_apiCountries.Code != 5200)
                {
                    return Ok(_apiCountries);
                }

                List<CountryModel> countries = [];
                foreach (var item in _apiCountries.Data)
                {
                    CountryModel country = new()
                    {
                        cou_key = item.country_key,
                        cou_name = item.country_name,
                        cou_iso2 = item.country_iso2,
                        cou_logo = item.country_logo,
                        cou_fec_creacion = DateTime.Now
                    };
                    countries.Add(country);
                }
                var _dbCountries = await _countryService.SyncCountries(countries);

                return Ok(_dbCountries);

            }
            catch (Exception ex)
            {
                var resultError = _utilities.HandleException(ex);
                result.Code = resultError.Code;
                result.Message = resultError.Message;
                result.Details = resultError.Details;
            }
            return Ok(result);
        }
        #endregion
    }
}
