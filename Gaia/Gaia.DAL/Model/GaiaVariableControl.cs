using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
    public class GaiaVariableControl
    {
        [Key]
        public string VariableControlId { get; set; }
        public string Descripcion { get; set; }
        public string Cadena { get; set; }
        public Nullable<DateTime> Fecha { get; set; }
        //public string Fecha { get; set; }
        public Nullable<int> Numerico { get; set; }
        public string TablaRel { get; set; }
        public string CampoRel { get; set; }
        public bool Activo { get; set; }
    }
}