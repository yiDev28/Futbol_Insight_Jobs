using EncryptorService.Services;
using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Models.DTO;
using Futbol_Insight_Jobs.Services.Access;
using Futbol_Insight_Jobs.Tools;
using LogServiceYiDev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Futbol_Insight_Jobs.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAccess _access;
        private readonly Utilities _utilities;
        public AccessController(IEncryptionService encryptionService, IAccess access, Utilities utilities)
        {
            _access = access;
            _utilities = utilities;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Registrarse(UserDTO user)
        {
            ResultModel<string> result = new ResultModel<string>();
            try
            {
                var r = await _access.RegistrarUsuario(user);

                result = r;

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

        [HttpPost]
        [Route("Signin")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            ResultModel<CredentialsUser> result = new ResultModel<CredentialsUser>();
            try
            {
                var r = await _access.IniciarSesion(user);
                result = r;
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
    }
}