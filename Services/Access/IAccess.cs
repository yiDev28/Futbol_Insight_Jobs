using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Futbol_Insight_Jobs.Services.Access
{
    public interface IAccess
    {
        Task<ResultModel<string>> RegistrarUsuario(UserDTO user);
        Task<ResultModel<CredentialsUser>> IniciarSesion(UserDTO user);
    }
}
