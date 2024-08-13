
using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Services.ApiService;
using Futbol_Insight_Jobs.Services.Country;
using Microsoft.AspNetCore.Mvc;

namespace Futbol_Insight_Jobs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountry _countryService;
        private readonly IApiService _apiService;


        public CountryController(ICountry countryService, IApiService apiService)
        {
            _countryService = countryService;
            _apiService = apiService;
        }
        #region Sincronizar Paises
        [HttpGet]
        [Route("/Refresh")]
        public async Task<IActionResult> RefrescarPaises()
        {
            try
            {
                List<ApiCountry> _apiCountries = await _apiService.GetCountries();

                List<CountryModel> countries = [];
                foreach (var item in _apiCountries)
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
            }
            catch
            {

            }
            return Ok();
        }
        #endregion
    }
}
