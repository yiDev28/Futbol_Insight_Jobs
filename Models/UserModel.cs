namespace Futbol_Insight_Jobs.Models
{
    public class UserModel
    {
        public string UsrNomUsuario { get; set; }
        public string UsrCorreo { get; set; }
        public string UsrContrasena { get; set; }
        public string UsrTelefono { get; set; }
        public string UsrTipoUsuario { get; set; }
        public string UsrEstado { get; set; }
        public string UsrUsuario { get; set; }
        public DateTime UsrFecCreacion { get; set; } = DateTime.Now;
        public DateTime UsrFecRegistro { get; set; } = DateTime.Now;

        // Ejemplo de lógica de negocio dentro del modelo
        public bool IsActive()
        {
            return UsrEstado == "Activo";
        }
    }
}
