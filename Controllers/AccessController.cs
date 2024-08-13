﻿using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Tools.Token;
using Futbol_Insight_Jobs.Models.DTO;
using Futbol_Insight_Jobs.Services.Access;


namespace Futbol_Insight_Jobs.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccessController : ControllerBase
    {
       private readonly Utilities _utilidades;
        private readonly IAccess _access;
        public AccessController(Utilities utilidades, IAccess access)
        {
            _utilidades = utilidades;
            _access = access;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UserDTO objeto)
        {
            //validar campo

            ////////////
            ///

            var modeloUsuario = new UserModel
            {
                UsrTelefono ="311123456",
                UsrCorreo="yidev28@gmail.com",
                UsrNomUsuario = objeto.User,
                UsrContrasena = _utilidades.encriptarSHA256(objeto.Pass),
                UsrUsuario="yiduard.bolivar.admin"
            };

            _access.RegistrarUsuario(modeloUsuario);
            return Ok();
        }

        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> Login(UserDTO objeto)
        //{
        //    var usuarioEncontrado = await _dbPruebaContext.Usuarios
        //                                            .Where(u =>
        //                                                u.Correo == objeto.Correo &&
        //                                                u.Clave == _utilidades.encriptarSHA256(objeto.Clave)
        //                                              ).FirstOrDefaultAsync();

        //    if (usuarioEncontrado == null)
        //        return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
        //    else
        //        return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
        //}
    }
}