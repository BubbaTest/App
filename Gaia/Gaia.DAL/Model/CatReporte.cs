using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DAL.Model
{
   public class catReporte
    {
        public catReporte () {
            //this.relRolReporte = new HashSet<relRolReporte>();
            this.relReporteCampo = new HashSet<relReporteCampo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ReporteId { get; set; }
        public string Descripcion { get; set; }
        public string NombreReporte { get; set; }
        public string SistemaId { get; set; }
        public string Modulo { get; set; }
        public bool Activo { get; set; }

        //[ForeignKey("ReporteId")]
        //public virtual ICollection<relRolReporte> relRolReporte { get; set; }

        //[ForeignKey("ReporteId")]
        public virtual ICollection<relReporteCampo> relReporteCampo { get; set; }
    }
}
