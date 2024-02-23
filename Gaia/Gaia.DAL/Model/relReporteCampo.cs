using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class relReporteCampo
    {
        [Column(Order = 0), Key]
        public decimal ReporteId { get; set; }
        [Column(Order = 1), Key]
        public decimal CampoReporteId { get; set; }
        public int Orden { get; set; }
        public bool Requerido { get; set; }
        public bool IsPrincipal { get; set; }

        [ForeignKey("ReporteId")]
        public virtual catReporte catReporte { get; set; }

        [ForeignKey("CampoReporteId")]
        public virtual CampoReporte CampoReporte { get; set; }
    }
}
