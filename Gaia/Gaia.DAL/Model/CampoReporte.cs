using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gaia.DAL.Model
{
    public class CampoReporte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal CampoReporteId { get; set; }
        public decimal TipoObjetoId { get; set; }
        public string Placeholder { get; set; }
        public string Titulo { get; set; }
        public decimal ParametroId { get; set; }
        public bool Activo { get; set; }
        public Nullable<bool> IsPrincipal { get; set; }

        [ForeignKey("TipoObjetoId")]
        public virtual catTipoObjeto catTipoObjeto { get; set; }

        [ForeignKey("ParametroId")]
        public virtual catParametro catParametro { get; set; }

        public virtual ICollection<relReporteCampo> relReporteCampo { get; set; }

    }
}
