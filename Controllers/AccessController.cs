using EncryptorService.Services;
using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Models.DTO;
using Futbol_Insight_Jobs.Services.Access;
using LogServiceYiDev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Futbol_Insight_Jobs.Controllers
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAccess _access;
        public AccessController(IEncryptionService encryptionService, IAccess access)
        {
            _access = access;
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

                result.Code = 5500;
                result.Message = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5500).Description;
                result.Details = ex.Message;
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

                result.Code = 5500;
                result.Message = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5500).Description;
                result.Details = ex.Message;
            }
            return Ok(result);
        }
    }
}