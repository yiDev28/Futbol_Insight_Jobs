using Futbol_Insight_Jobs.Data;
using Futbol_Insight_Jobs.Models;
using Microsoft.EntityFrameworkCore;

namespace Futbol_Insight_Jobs.Services.Country
{
    public class Country : ICountry
    {
        private readonly DataContext _context;

        public Country(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> SyncCountries(List<CountryModel> countries)
        {
            try
            {
                // Obtener las claves de los países que estamos insertando
                var keys = countries.Select(c => c.cou_key).ToList();

                // Buscar los países existentes en la base de datos
                var existingCountriesInDb = await _context.countries
                    .Where(c => keys.Contains(c.cou_key))
                    .ToListAsync();

                // Crear un diccionario de países existentes por cou_key
                var existingCountriesDict = existingCountriesInDb.ToDictionary(c => c.cou_key);

                // Listas para nuevos países y países a actualizar
                var newCountries = new List<CountryModel>();
                var updatedCountries = new List<CountryModel>();

                foreach (var country in countries)
                {
                    if (existingCountriesDict.TryGetValue(country.cou_key, out var existingCountry))
                    {

                        // Verificar si alguno de los campos ha cambiado
                        bool isUpdated = false;

                        if (existingCountry.cou_iso2 != country.cou_iso2)
                        {
                            existingCountry.cou_iso2 = country.cou_iso2;
                            isUpdated = true;
                        }
                        if (existingCountry.cou_name != country.cou_name)
                        {
                            existingCountry.cou_name = country.cou_name;
                            isUpdated = true;
                        }
                        if (existingCountry.cou_logo != country.cou_logo)
                        {
                            existingCountry.cou_logo = country.cou_logo;
                            isUpdated = true;
                        }

                        if (isUpdated)
                        {
                            existingCountry.cou_fec_registro = country.cou_fec_registro;
                            updatedCountries.Add(existingCountry);
                        }
                    }
                    else
                    {
                        // Agregar nuevo país
                        newCountries.Add(country);
                    }
                }

                // Insertar nuevos países
                if (newCountries.Count > 0)
                {
                    _context.countries.AddRange(newCountries);
                }

                // Guardar los cambios
                var r = await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Registrar el error para depuración
                // _logger.LogError(ex, "Error syncing countries");
                throw new Exception("Error syncing countries", ex);
            }
        }

    }
}
