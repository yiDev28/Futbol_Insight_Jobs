using System.ComponentModel.DataAnnotations;

namespace Futbol_Insight_Jobs.Models
{
    public class UserModel
    {
        [Key]
        public int usr_id_int { get; set; }
        public string usr_nom_usuario { get; set; }
        public string usr_contrasena { get; set; }
        public int usr_tipo_usuario { get; set; }
        public string? usr_ult_token { get; set; }
        public int usr_estado { get; set; } = 0;
        public int usr_usuario { get; set; }
        public DateTime usr_fec_creacion { get; set; } = DateTime.Now;
        public DateTime usr_fec_registro { get; set; } = DateTime.Now;
    }  

    public class CredentialsUser
    {
        public string Token { get; set; }
    }
}
