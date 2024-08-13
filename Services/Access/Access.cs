using Futbol_Insight_Jobs.Data;
using Futbol_Insight_Jobs.Models;

namespace Futbol_Insight_Jobs.Services.Access
{
    public class Access : IAccess
    {
        private readonly AdmonContext _context;

        public Access(AdmonContext context)
        {
            _context = context;
        }

   

        public async Task<ResultModel<bool>> RegistrarUsuario(UserModel user)
        {
            var r = _context.AddAsync(user);

            var _r = await _context.SaveChangesAsync();

            return new ResultModel<bool>(200, "Countries synced successfully", true);
        }
    }
}
