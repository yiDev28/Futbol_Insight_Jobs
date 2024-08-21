using EncryptorService.Services;
using Futbol_Insight_Jobs.Data;
using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Models.DTO;
using Futbol_Insight_Jobs.Tools;
using LogServiceYiDev.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Futbol_Insight_Jobs.Services.Access
{
    public class Access : IAccess
    {
        private readonly AdmonContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly Utilities _utilities;
        public Access(AdmonContext context, IEncryptionService encryptionService, Utilities utilities)
        {
            _context = context;
            _encryptionService = encryptionService;
            _utilities = utilities;

        }

        public async Task<ResultModel<CredentialsUser>> IniciarSesion(UserDTO user)
        {
            ResultModel<CredentialsUser> result = new ResultModel<CredentialsUser>();
            try
            {
                var usuarioEncontrado = await _context.users_jobs
                                                   .Where(u =>
                                                       u.usr_nom_usuario == user.User &&
                                                       u.usr_contrasena == _encryptionService.encriptarSHA256(user.Pass)
                                                     ).FirstOrDefaultAsync();
                if (usuarioEncontrado == null)
                {
                    result.Code = 5401;
                    result.Message = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5401).UserMessage;
                    result.Details = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5401).Description;

                }
                else
                {
                    string token = _utilities.generarJWT(usuarioEncontrado);
                    // Actualizar el campo token del usuario
                    usuarioEncontrado.usr_ult_token = token;
                    usuarioEncontrado.usr_fec_registro = DateTime.Now;
                    // Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();

                    result.Code = 5200;
                    result.Message = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5200)?.UserMessage;
                    result.Details = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5200).Description;
                    result.Data = new CredentialsUser { Token = token };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<ResultModel<string>> RegistrarUsuario(UserDTO user)
        {
            ResultModel<string> result = new ResultModel<string>(5200, ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5200).UserMessage, "");
            try
            {
                var modelUser = new UserModel
                {
                    usr_nom_usuario = user.User,
                    usr_contrasena = _encryptionService.encriptarSHA256(user.Pass),
                    usr_usuario = 9999,
                    usr_tipo_usuario = 888,
                };
                await _context.users_jobs.AddAsync(modelUser);
                var _r = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
