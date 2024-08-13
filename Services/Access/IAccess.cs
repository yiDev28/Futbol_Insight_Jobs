using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Futbol_Insight_Jobs.Services.Access
{
    public interface IAccess
    {
        Task<ResultModel<bool>> RegistrarUsuario(UserModel user);
    }
}
