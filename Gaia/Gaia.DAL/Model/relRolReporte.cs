using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class relRolReporte
    {
        //public relRolReporte() {
        //    this.Reportes = new HashSet<catReporte>();
        //}
        [Column(Order = 0), Key]
        public string RolId { get; set; }
        [Column(Order = 1), Key]
        public decimal ReporteId { get; set; }

        [ForeignKey("ReporteId")]
        public virtual catReporte Reportes { get; set; }

    }
}
