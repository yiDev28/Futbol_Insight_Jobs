using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futbol_Insight_Jobs.Models
{
    public class CountryModels
    {
    }
    public class CountryModel
    {
        [Key]
        public int cou_id_int { get; set; }
        public int cou_key { get; set; }
        public string cou_name { get; set; }
        public string? cou_iso2 { get; set; }
        public string? cou_logo { get; set; }
        public string cou_estado { get; set; } = "ACT";
        public string cou_usuario { get; set; } = "systemJobs";
        public DateTime? cou_fec_creacion { get; set; } 
        public DateTime cou_fec_registro { get; set; } = DateTime.Now;
    }
}
