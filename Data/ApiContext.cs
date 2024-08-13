using Microsoft.Extensions.Configuration;
using System.Web;

namespace Futbol_Insight_Jobs.Data
{
    public class ApiContext
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiContext(HttpClient httpClient,IConfiguration configuration )
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> GetApiAllSport(string module)
        {
            try
            {
                var builder = new UriBuilder(_configuration.GetValue<string>("ApiData:Url"));
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["met"] = module;
                query["APIkey"] = _configuration.GetValue<string>("ApiData:Key");
                builder.Query = query.ToString();

                var response = await _httpClient.GetAsync(builder.ToString());

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"error al consumir el api: {ex.Message}");
            }
        }
    }
}
