using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Gaia.DAL.Model
{
    public class vwRelUsuarioRolEntidadNicarao
    {
        [Key]
        public string UsuarioId { get; set; }
        public string RolId { get; set; }
        public string RolDescripcion { get; set; }
        public string EntidadId { get; set; }
        public string EntidadDescripcion { get; set; }
        public Guid relGUID { get; set; }
        
    }
}
