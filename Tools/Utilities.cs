using Futbol_Insight_Jobs.Models;
using Futbol_Insight_Jobs.Models.DTO;
using LogServiceYiDev.Models;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Futbol_Insight_Jobs.Tools
{
    public class Utilities
    {
        private readonly IConfiguration _configuration;
        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string generarJWT(UserModel user)
        {
            //crear la informacion del usuario para token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.usr_nom_usuario.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(3600),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        public  ResultBase HandleException(Exception ex)
        {
            var result = new ResultBase();

            if (ex.InnerException is MySqlException sqlEx)
            {
                result.Code = sqlEx.Number;
                result.Message = ErrorDictionary.Errors
                    .FirstOrDefault(e => e.ErrorCode == sqlEx.Number)?.UserMessage
                    ?? sqlEx.ErrorCode.ToString();
                result.Details = sqlEx.Message;
            }
            else
            {
                result.Code = 5500;
                result.Message = ErrorDictionary.Errors.FirstOrDefault(e => e.ErrorCode == 5500).Description;
                result.Details = ex.Message;
            }

            return result;
        }

    }
}
